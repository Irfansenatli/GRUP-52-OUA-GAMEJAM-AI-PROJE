
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class ApiActivate : MonoBehaviour
{
    public TMP_InputField chatInputField;
    public TextMeshProUGUI chatOutputText;
    private string apiKey = "AIzaSyDr4774yt5SqYm3hQuGtBgBH6fYgUSXWuE"; // API anahtarýnýzý buraya ekleyin
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"; // Uç nokta URL'si

    private int questionCount = 0;
    public static bool hasAnsweredCorrectly = false;
    private string initialPrompt = "senin adýn Gandalf, sana bir zaman yolcusu gelecek, ona hoþgeldin de.";

    private string correctAnswer = "arin zamanýn koruyucusu olarak evrenin düzenini ve dengesini saðlamaktan sorumluydu"; // Doðru cevap

    private void Start()
    {
        StartCoroutine(InitializeChatbot());
    }

    public void OnSubmit()
    {
        string inputText = chatInputField.text.ToLower();
        StartCoroutine(GetChatbotResponse(inputText));
    }

    private IEnumerator InitializeChatbot()
    {
        // API'ye JSON formatýnda veri gönderin
        string jsonPayload = "{\"contents\":[{\"parts\":[{\"text\":\"" + initialPrompt + "\"}]}]}";
        string fullUrl = apiUrl + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                chatOutputText.text = "Error: " + request.error;
            }
            else
            {
                if (request.responseCode == 401)
                {
                    Debug.LogError("Unauthorized: Check your API key and permissions.");
                    chatOutputText.text = "Error: Unauthorized. Check your API key and permissions.";
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Server Response: " + jsonResponse);

                    // JSON yanýtýný iþleyin ve çýktý metnini ayarlayýn
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    string chatbotText = response.candidates[0].content.parts[0].text;
                    chatOutputText.text = TrimToMaxLength(chatbotText, 400);

                    // Ýlk bilmecenizi API'den alýn
                    StartCoroutine(GetRandomRiddle());
                }
            }
        }
    }

    private IEnumerator GetRandomRiddle()
    {
        if (hasAnsweredCorrectly)
        {
            yield break; // Doðru cevap verildiyse baþka bilmece sorma
        }

        string riddlePrompt = "Sadece Þu soruyu sor: Sen kimsin ve sorumluluðun neydi? (Doðru Cevap Oda içerisindeki yazýtlarda olabilir) Cevap: Arin zamanýn koruyucusu olarak evrenin düzenini ve dengesini saðlamaktan sorumluydu CEVABI SÖYLEME.";
        string jsonPayload = "{\"contents\":[{\"parts\":[{\"text\":\"" + riddlePrompt + "\"}]}]}";
        string fullUrl = apiUrl + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                chatOutputText.text = "Error: " + request.error;
            }
            else
            {
                if (request.responseCode == 401)
                {
                    Debug.LogError("Unauthorized: Check your API key and permissions.");
                    chatOutputText.text = "Error: Unauthorized. Check your API key and permissions.";
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Server Response: " + jsonResponse);

                    // JSON yanýtýný iþleyin ve çýktý metnini ayarlayýn
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    string chatbotText = response.candidates[0].content.parts[0].text;
                    chatOutputText.text += "\n" + TrimToMaxLength(chatbotText, 200);
                }
            }
        }
    }

    private IEnumerator GetChatbotResponse(string inputText)
    {
        if (questionCount >= 5 || hasAnsweredCorrectly)
        {
            chatOutputText.text = "Üzgünüm, artýk baþka bir soru kabul etmiyorum.";
            yield break;
        }

        // API'ye JSON formatýnda veri gönderin
        string jsonPayload = "{\"contents\":[{\"parts\":[{\"text\":\"" + inputText + "\"}]}]}";
        string fullUrl = apiUrl + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                chatOutputText.text = "Error: " + request.error;
            }
            else
            {
                if (request.responseCode == 401)
                {
                    Debug.LogError("Unauthorized: Check your API key and permissions.");
                    chatOutputText.text = "Error: Unauthorized. Check your API key and permissions.";
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Server Response: " + jsonResponse);

                    // JSON yanýtýný iþleyin ve çýktý metnini ayarlayýn
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    string chatbotText = response.candidates[0].content.parts[0].text;
                    chatOutputText.text = TrimToMaxLength(chatbotText, 200);

                    // Cevabý kontrol edin
                    bool isCorrect = CheckAnswer(inputText);
                    if (isCorrect)
                    {
                        chatOutputText.text += "\nTebrikler, doðru cevap verdiniz! Kapý açýldý.";
                        hasAnsweredCorrectly = true; // Doðru cevap verildiðini iþaretle
                    }
                    else
                    {
                        chatOutputText.text += "\nYanlýþ cevap, lütfen tekrar deneyin.";
                    }

                    if (questionCount < 5 && !hasAnsweredCorrectly)
                    {
                        questionCount++;
                        StartCoroutine(GetRandomRiddle());
                    }
                }
            }
        }
    }

    private bool CheckAnswer(string userAnswer)
    {
        return userAnswer.Trim().ToLower() == correctAnswer.Trim().ToLower();
    }

    private string TrimToMaxLength(string text, int maxLength)
    {
        if (text.Length > maxLength)
        {
            return text.Substring(0, maxLength);
        }
        return text;
    }

    [System.Serializable]
    private class ChatbotResponse
    {
        public Candidate[] candidates;
    }

    [System.Serializable]
    private class Candidate
    {
        public Content content;
    }

    [System.Serializable]
    private class Content
    {
        public Part[] parts;
    }

    [System.Serializable]
    private class Part
    {
        public string text;
    }
}
























/*
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class ApiActivate : MonoBehaviour
{
    public TMP_InputField chatInputField;
    public TextMeshProUGUI chatOutputText;
    private string apiKey = "AIzaSyDr4774yt5SqYm3hQuGtBgBH6fYgUSXWuE"; // API anahtarýnýzý buraya ekleyin
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"; // Uç nokta URL'si

    private int questionCount = 0;
    public static bool hasAnsweredCorrectly = false;
    private string initialPrompt = "Sen bu tapýnakta insanlarýn anýlarýndan sorumlu dedesin. Seninle konuþan kiþi eski bir zaman yolcusu ve zamanýn içindeki kayýp anýlarýný geri kazanmak istiyor. Diðer odalara gidip geçmiþini öðrenmesi için soruna doðru cevap vermeli. Sadece doðru cevap verirse doðru cevap verdiðini ve devam edebileceðini söyle.";

    private string correctAnswer = "Arin zamanýn koruyucusu olarak evrenin düzenini ve dengesini saðlamaktan sorumluydu"; // Doðru cevap

    private void Start()
    {
        StartCoroutine(InitializeChatbot());
    }

    public void OnSubmit()
    {
        string inputText = chatInputField.text;
        StartCoroutine(GetChatbotResponse(inputText));
    }

    private IEnumerator InitializeChatbot()
    {
        // API'ye JSON formatýnda veri gönderin
        string jsonPayload = "{\"contents\":[{\"parts\":[{\"text\":\"" + initialPrompt + "\"}]}]}";
        string fullUrl = apiUrl + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                chatOutputText.text = "Error: " + request.error;
            }
            else
            {
                if (request.responseCode == 401)
                {
                    Debug.LogError("Unauthorized: Check your API key and permissions.");
                    chatOutputText.text = "Error: Unauthorized. Check your API key and permissions.";
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Server Response: " + jsonResponse);

                    // JSON yanýtýný iþleyin ve çýktý metnini ayarlayýn
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    chatOutputText.text = response.candidates[0].content.parts[0].text; // Yanýt formatýna göre doðru alaný kullanýn

                    // Ýlk bilmecenizi API'den alýn
                    StartCoroutine(GetRandomRiddle());
                }
            }
        }
    }

    private IEnumerator GetRandomRiddle()
    {
        if (hasAnsweredCorrectly)
        {
            yield break; // Doðru cevap verildiyse baþka bilmece sorma
        }

        string riddlePrompt = "Þu soruyu sor, Sen kimsin ve sorumluluðun neydi? Cevap: Arin zamanýn koruyucusu olarak evrenin düzenini ve dengesini saðlamaktan sorumluydu CEVABI SÖYLEME.";
        string jsonPayload = "{\"contents\":[{\"parts\":[{\"text\":\"" + riddlePrompt + "\"}]}]}";
        string fullUrl = apiUrl + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                chatOutputText.text = "Error: " + request.error;
            }
            else
            {
                if (request.responseCode == 401)
                {
                    Debug.LogError("Unauthorized: Check your API key and permissions.");
                    chatOutputText.text = "Error: Unauthorized. Check your API key and permissions.";
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Server Response: " + jsonResponse);

                    // JSON yanýtýný iþleyin ve çýktý metnini ayarlayýn
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    chatOutputText.text += "\n" + response.candidates[0].content.parts[0].text; // Yanýt formatýna göre doðru alaný kullanýn
                }
            }
        }
    }

    private IEnumerator GetChatbotResponse(string inputText)
    {
        if (questionCount >= 5 || hasAnsweredCorrectly)
        {
            chatOutputText.text = "Üzgünüm, artýk baþka bir soru kabul etmiyorum.";
            yield break;
        }
       
        // API'ye JSON formatýnda veri gönderin
        string jsonPayload = "{\"contents\":[{\"parts\":[{\"text\":\"" + inputText + "\"}]}]}";
        string fullUrl = apiUrl + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                chatOutputText.text = "Error: " + request.error;
            }
            else
            {
                if (request.responseCode == 401)
                {
                    Debug.LogError("Unauthorized: Check your API key and permissions.");
                    chatOutputText.text = "Error: Unauthorized. Check your API key and permissions.";
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Server Response: " + jsonResponse);

                    // JSON yanýtýný iþleyin ve çýktý metnini ayarlayýn
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    chatOutputText.text = response.candidates[0].content.parts[0].text; // Yanýt formatýna göre doðru alaný kullanýn

                    // Cevabý kontrol edin
                    bool isCorrect = CheckAnswer(inputText);
                    if (isCorrect)
                    {
                        chatOutputText.text += "\nTebrikler, doðru cevap verdiniz! Kapý açýldý.";
                        hasAnsweredCorrectly = true; // Doðru cevap verildiðini iþaretle
                    }
                    else
                    {
                        chatOutputText.text += "\nYanlýþ cevap, lütfen tekrar deneyin.";
                    }

                    if (questionCount < 5 && !hasAnsweredCorrectly)
                    {
                        questionCount++;
                        StartCoroutine(GetRandomRiddle());
                    }
                }
            }
        }
    }

    private bool CheckAnswer(string userAnswer)
    {
        return userAnswer.Trim().ToLower() == correctAnswer.Trim().ToLower();
    }

    [System.Serializable]
    private class ChatbotResponse
    {
        public Candidate[] candidates;
    }

    [System.Serializable]
    private class Candidate
    {
        public Content content;
    }

    [System.Serializable]
    private class Content
    {
        public Part[] parts;
    }

    [System.Serializable]
    private class Part
    {
        public string text;
    }
}
*/


