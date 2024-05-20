using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class ApiActivate4 : MonoBehaviour
{
    public TMP_InputField chatInputField;
    public TextMeshProUGUI chatOutputText;
    private string apiKey = "AIzaSyDr4774yt5SqYm3hQuGtBgBH6fYgUSXWuE"; // API anahtar�n�z� buraya ekleyin
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"; // U� nokta URL'si

    private int questionCount = 0;
    public static bool hasAnsweredCorrectly = false;
    private string initialPrompt = "senin ad�n Gandalf, sana bir zaman yolcusu gelecek, ona ho�geldin de.";

    private string correctAnswer = "zaman�n koruyucusu olarak yeni maceralara ve zaman�n s�rlar�n� ke�fetmeye devam edecekti"; // Do�ru cevap

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
        // API'ye JSON format�nda veri g�nderin
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

                    // JSON yan�t�n� i�leyin ve ��kt� metnini ayarlay�n
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    string chatbotText = response.candidates[0].content.parts[0].text;
                    chatOutputText.text = TrimToMaxLength(chatbotText, 400);

                    // �lk bilmecenizi API'den al�n
                    StartCoroutine(GetRandomRiddle());
                }
            }
        }
    }

    private IEnumerator GetRandomRiddle()
    {
        if (hasAnsweredCorrectly)
        {
            yield break; // Do�ru cevap verildiyse ba�ka bilmece sorma
        }

        string riddlePrompt = "Sadece �u soruyu sor: Arin, zaman�n koruyucusu olarak tap�naktan ayr�ld�ktan sonra ne yapmaya devam etti? (Do�ru Cevap Oda i�erisindeki yaz�tlarda olabilir) Cevap: Zaman�n koruyucusu olarak yeni maceralara ve zaman�n s�rlar�n� ke�fetmeye devam edecekti  CEVABI S�YLEME.";
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

                    // JSON yan�t�n� i�leyin ve ��kt� metnini ayarlay�n
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    string chatbotText = response.candidates[0].content.parts[0].text;
                    chatOutputText.text = TrimToMaxLength(chatbotText, 400);
                }
            }
        }
    }

    private IEnumerator GetChatbotResponse(string inputText)
    {
        if (questionCount >= 5 || hasAnsweredCorrectly)
        {
            chatOutputText.text = "�zg�n�m, art�k ba�ka bir soru kabul etmiyorum.";
            yield break;
        }

        // API'ye JSON format�nda veri g�nderin
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

                    // JSON yan�t�n� i�leyin ve ��kt� metnini ayarlay�n
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    string chatbotText = response.candidates[0].content.parts[0].text;
                    chatOutputText.text = TrimToMaxLength(chatbotText, 200);

                    // Cevab� kontrol edin
                    bool isCorrect = CheckAnswer(inputText);
                    if (isCorrect)
                    {
                        chatOutputText.text += "\nTebrikler, do�ru cevap verdiniz! Kap� a��ld�.";
                        hasAnsweredCorrectly = true; // Do�ru cevap verildi�ini i�aretle
                    }
                    else
                    {
                        chatOutputText.text += "\nYanl�� cevap, l�tfen tekrar deneyin.";
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

























/*using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class ApiActivate4 : MonoBehaviour
{
    public TMP_InputField chatInputField;
    public TextMeshProUGUI chatOutputText;
    private string apiKey = "AIzaSyDr4774yt5SqYm3hQuGtBgBH6fYgUSXWuE"; // API anahtar�n�z� buraya ekleyin
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"; // U� nokta URL'si

    private int questionCount = 0;
    public static bool hasAnsweredCorrectly = false;
    private string initialPrompt = "Sen bu tap�nakta insanlar�n an�lar�ndan sorumlu dedesin. Seninle konu�an ki�i eski bir zaman yolcusu ve zaman�n i�indeki kay�p an�lar�n� geri kazanmak istiyor. Di�er odalara gidip ge�mi�ini ��renmesi i�in soruna do�ru cevap vermeli. Sadece do�ru cevap verirse do�ru cevap verdi�ini ve devam edebilece�ini s�yle.";

    private string correctAnswer = "Zaman�n koruyucusu olarak yeni maceralara ve zaman�n s�rlar�n� ke�fetmeye devam edecekti"; // Do�ru cevap

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
        string jsonPayload = "{\"contents\":[{\"parts\":[{\"text\":\"" + initialPrompt + "\"}]}]}";
        string fullUrl = apiUrl + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Sending initial prompt to API...");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                chatOutputText.text = "Error: " + request.error;
            }
            else
            {
                Debug.Log("Initial prompt sent successfully.");
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Server Response: " + jsonResponse);

                ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                if (response != null && response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                {
                    chatOutputText.text = response.candidates[0].content.parts[0].text;
                }
                else
                {
                    Debug.LogError("Invalid response format.");
                }

                StartCoroutine(GetRandomRiddle());
            }
        }
    }

    private IEnumerator GetRandomRiddle()
    {
        if (hasAnsweredCorrectly)
        {
            yield break; // Do�ru cevap verildiyse ba�ka bilmece sorma
        }

        string riddlePrompt = "�u soruyu sor, Arin, zaman�n koruyucusu olarak tap�naktan ayr�ld�ktan sonra ne yapmaya devam etti? (Do�ru Cevap Oda i�erisindeki yaz�tlarda olabilir) Cevap: Zaman�n koruyucusu olarak yeni maceralara ve zaman�n s�rlar�n� ke�fetmeye devam edecekti  CEVABI S�YLEME.";
        string jsonPayload = "{\"contents\":[{\"parts\":[{\"text\":\"" + riddlePrompt + "\"}]}]}";
        string fullUrl = apiUrl + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Sending riddle prompt to API...");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                chatOutputText.text = "Error: " + request.error;
            }
            else
            {
                Debug.Log("Riddle prompt sent successfully.");
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Server Response: " + jsonResponse);

                ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                if (response != null && response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                {
                    chatOutputText.text += "\n" + response.candidates[0].content.parts[0].text;
                }
                else
                {
                    Debug.LogError("Invalid response format.");
                }
            }
        }
    }

    private IEnumerator GetChatbotResponse(string inputText)
    {
        if (questionCount >= 5 || hasAnsweredCorrectly)
        {
            chatOutputText.text = "�zg�n�m, art�k ba�ka bir soru kabul etmiyorum.";
            yield break;
        }
       
        string jsonPayload = "{\"contents\":[{\"parts\":[{\"text\":\"" + inputText + "\"}]}]}";
        string fullUrl = apiUrl + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Sending user input to API...");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                chatOutputText.text = "Error: " + request.error;
            }
            else
            {
                Debug.Log("User input sent successfully.");
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Server Response: " + jsonResponse);

                ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                if (response != null && response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                {
                    string botResponse = response.candidates[0].content.parts[0].text;
                    chatOutputText.text = botResponse;

                    bool isCorrect = CheckAnswer(inputText);
                    if (isCorrect)
                    {
                        chatOutputText.text += "\nTebrikler, do�ru cevap verdiniz! Kap� a��ld�.";
                        hasAnsweredCorrectly = true; // Do�ru cevap verildi�ini i�aretle
                    }
                    else
                    {
                        chatOutputText.text += "\nYanl�� cevap, l�tfen tekrar deneyin.";
                    }

                    if (questionCount < 5 && !hasAnsweredCorrectly)
                    {
                        questionCount++;
                        StartCoroutine(GetRandomRiddle());
                    }
                }
                else
                {
                    Debug.LogError("Invalid response format.");
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

