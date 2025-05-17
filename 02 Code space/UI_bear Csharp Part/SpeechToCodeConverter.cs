using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NAudio.Wave;
using Microsoft.VisualBasic;

namespace SpeechToCodeTest
{
    public class AudioRecorder
    {
        private WaveInEvent waveIn;
        private WaveFileWriter writer;
        private string filePath;
        private MemoryStream memoryStream;

        public string FilePath => filePath;

        public AudioRecorder()
        {
            memoryStream = new MemoryStream();
        }

        public void StartRecording(string filePath)
        {
            try
            {
                this.filePath = filePath;
                waveIn = new WaveInEvent
                {
                    WaveFormat = new WaveFormat(16000, 1)
                };

                writer = new WaveFileWriter(memoryStream, waveIn.WaveFormat);
                waveIn.DataAvailable += (s, e) => writer.Write(e.Buffer, 0, e.BytesRecorded);
                waveIn.StartRecording();
            }
            catch (Exception ex)
            {
                throw new Exception("录音启动失败：" + ex.Message);
            }
        }

        public async Task<byte[]> StopRecordingAsync()
        {
            try
            {
                waveIn?.StopRecording();
                waveIn?.Dispose();
                writer?.Close();

                byte[] audioData = memoryStream.ToArray();
                await File.WriteAllBytesAsync(filePath, audioData);
                return audioData;
            }
            catch (Exception ex)
            {
                throw new Exception("停止录音失败：" + ex.Message);
            }
            finally
            {
                memoryStream?.Dispose();
                memoryStream = new MemoryStream();
            }
        }
    }

    public class BaiduSpeechRecognizer
    {
        private readonly string apiKey = "HeUJgw8vAMTavD7hGLybm9w4";
        private readonly string secretKey = "MXCTz99mxVPKtzlAckgVzlpPaWiRnkpa";
        private readonly string cuid = "my-unique-cuid-12345";
        private string accessToken;
        private DateTime tokenExpiration;
        private readonly HttpClient authClient;

        public BaiduSpeechRecognizer()
        {
            authClient = new HttpClient();
        }

        public async Task<bool> AuthenticateAsync(Action<string> updateStatus)
        {
            if (!string.IsNullOrEmpty(accessToken) && DateTime.UtcNow < tokenExpiration)
            {
                updateStatus("使用缓存的认证");
                return true;
            }

            updateStatus("正在认证......");
            try
            {
                string url = $"https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={apiKey}&client_secret={secretKey}";
                var response = await authClient.GetStringAsync(url);
                var json = JObject.Parse(response);
                accessToken = json["access_token"]?.ToString() ?? throw new Exception("获取 Access Token 失败");
                tokenExpiration = DateTime.UtcNow.AddSeconds(json["expires_in"]?.ToObject<int>() ?? 2592000);
                updateStatus("认证成功");
                return true;
            }
            catch (Exception ex)
            {
                updateStatus("认证失败：" + ex.Message);
                return false;
            }
        }

        public async Task<string> RecognizeSpeechAsync(byte[] audioBytes, Action<string> updateStatus)
        {
            updateStatus("正在识别......");
            try
            {
                string audioBase64 = Convert.ToBase64String(audioBytes);
                string url = "http://vop.baidu.com/server_api";
                var jsonContent = $@"{{
                    ""format"": ""wav"",
                    ""rate"": 16000,
                    ""channel"": 1,
                    ""cuid"": ""{cuid}"",
                    ""token"": ""{accessToken}"",
                    ""dev_pid"": 1537,
                    ""speech"": ""{audioBase64}"",
                    ""len"": {audioBytes.Length}
                }}";
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(result);

                    if (json["err_no"]?.ToString() == "0")
                    {
                        updateStatus("识别完成");
                        return json["result"]?[0]?.ToString() ?? "识别为空";
                    }
                    else
                    {
                        updateStatus($"识别失败：{json["err_msg"]}");
                        return $"识别失败：{json["err_msg"]} (错误码: {json["err_no"]})";
                    }
                }
            }
            catch (Exception ex)
            {
                updateStatus("识别出错：" + ex.Message);
                throw;
            }
        }
    }

    public class InstructionGenerator_DS_Faster : IDisposable
    {
        private readonly string deepSeekApiKey = "sk-fdc5df511b2d4a18b02f7f8e7a03a637";
        private readonly string instructionSet;
        private readonly HttpClient client;
        private readonly List<dynamic> conversationHistory;
        private readonly int maxHistoryRounds = 5; // 限制历史轮次

        public InstructionGenerator_DS_Faster(string instructionFilePath)
        {
            instructionSet = File.Exists(instructionFilePath) ? File.ReadAllText(instructionFilePath) : GetDefaultInstructionSet();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {deepSeekApiKey}");
            conversationHistory = new List<dynamic>();
        }

        private string GetDefaultInstructionSet() => @"
            goline x
            turn x
            circle x y
            for x s endfor
            if q s endif
            while q s endwhile
            break
            mov x q
            undefcode
            undefexpr";

        private string GetSystemPrompt() => $"你是一个代码生成助手，将自然语言转为以下指令：{instructionSet}。要求：仅输出指令，无缩进，基于之前的对话生成连贯的指令。";

        public async Task<string> GenerateInstructionsAsync(string speechText, Action<string> updateStatus)
        {
            updateStatus("正在生成指令");
            try
            {
                // 添加用户输入到历史
                conversationHistory.Add(new { role = "user", content = speechText });

                // 构造消息列表，包含系统提示和历史对话
                var messages = new List<dynamic> { new { role = "system", content = GetSystemPrompt() } };
                // 只取最近 maxHistoryRounds 轮对话
                int startIndex = Math.Max(0, conversationHistory.Count - maxHistoryRounds * 2);
                messages.AddRange(conversationHistory.GetRange(startIndex, conversationHistory.Count - startIndex));

                var requestBody = new
                {
                    model = "deepseek-chat",
                    messages,
                    temperature = 0.3
                };

                var jsonRequest = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.deepseek.com/v1/chat/completions", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"DeepSeek API 调用失败：{responseString}");

                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseString);
                string instructions = jsonResponse.choices[0].message.content.ToString().Trim();

                // 添加模型输出到历史
                conversationHistory.Add(new { role = "assistant", content = instructions });

                updateStatus("指令生成完成");
                return instructions;
            }
            catch (Exception ex)
            {
                updateStatus("指令生成失败：" + ex.Message);
                throw;
            }
        }

        public void ClearHistory()
        {
            conversationHistory.Clear();
        }

        public void Dispose() => client.Dispose();
    }

    public class SpeechToCodeConverter : IDisposable
    {
        private readonly AudioRecorder recorder;
        private readonly BaiduSpeechRecognizer speechRecognizer;
        private readonly InstructionGenerator_DS_Faster instructionGenerator;
        private bool isAuthenticated = false;

        public SpeechToCodeConverter(string referenceFilePath = "il.txt")
        {
            recorder = new AudioRecorder();
            speechRecognizer = new BaiduSpeechRecognizer();
            instructionGenerator = new InstructionGenerator_DS_Faster(referenceFilePath);
        }

        public async Task RecordAudioAsync(bool isRecording, string audioFilePath, Action<string> updateStatus)
        {
            try
            {
                if (isRecording)
                {
                    updateStatus("正在录音......");
                    recorder.StartRecording(audioFilePath);
                }
                else
                {
                    updateStatus("录音已停止，保存中");
                    await recorder.StopRecordingAsync();
                    updateStatus("录音保存完成");
                }
            }
            catch (Exception ex)
            {
                updateStatus(isRecording ? "录音启动失败：" + ex.Message : "录音停止失败：" + ex.Message);
                throw;
            }
        }

        // 用于文本显示
        public async Task<string> RecognizeSpeechOnlyAsync(string audioFilePath, Action<string> updateStatus)
        {
            try
            {
                if (!isAuthenticated)
                {
                    isAuthenticated = await speechRecognizer.AuthenticateAsync(updateStatus);
                    if (!isAuthenticated)
                        return "认证失败，无法继续";
                }

                byte[] audioBytes = await File.ReadAllBytesAsync(audioFilePath);
                return await speechRecognizer.RecognizeSpeechAsync(audioBytes, updateStatus);
            }
            catch (Exception ex)
            {
                updateStatus("语音识别出错：" + ex.Message);
                throw;
            }
        }

        public async Task<string> ConvertAudioToInstructionsAsync(string audioFilePath, string instructionFilePath, Action<string> updateStatus)
        {
            try
            {
                if (!isAuthenticated)
                {
                    isAuthenticated = await speechRecognizer.AuthenticateAsync(updateStatus);
                    if (!isAuthenticated)
                        return "认证失败，无法继续";
                }

                byte[] audioBytes = await File.ReadAllBytesAsync(audioFilePath);
                Task<string> recognizeTask = speechRecognizer.RecognizeSpeechAsync(audioBytes, updateStatus);
                string recognizedText = await recognizeTask;

                string instructions = await instructionGenerator.GenerateInstructionsAsync(recognizedText, updateStatus);

                updateStatus("正在保存指令");
                await File.AppendAllTextAsync(instructionFilePath, instructions + Environment.NewLine); // 追加指令
                updateStatus("保存已经完成");

                return instructions;
            }
            catch (Exception ex)
            {
                updateStatus("转换过程出错：" + ex.Message);
                throw;
            }
        }

        public void ClearConversationHistory()
        {
            instructionGenerator.ClearHistory();
        }

        public void Dispose()
        {
            instructionGenerator?.Dispose();
        }
    }
}