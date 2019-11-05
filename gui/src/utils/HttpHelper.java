package utils;

import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

public class HttpHelper {

    public static JSONObject Get(String targetURL) {
        return doRequest(targetURL, "GET", null);
    }

    public static JSONObject Post(String targetURL, JSONObject data)
    {
        return doRequest(targetURL, "POST", data);
    }

    public static JSONObject Put(String targetURL, JSONObject data)
    {
        return doRequest(targetURL, "PUT", data);
    }

    private static JSONObject doRequest(String targetURL, String method, JSONObject data)
    {
        JSONObject jsonObject = null;

        try {
            URL url = new URL(targetURL);
            HttpURLConnection httpClient = (HttpURLConnection) url.openConnection();
            httpClient.setRequestMethod(method);
            httpClient.setRequestProperty("Content-Type", "Application/json");
            httpClient.setRequestProperty("Accept-Type", "Application/json");

            if (data != null) {
                httpClient.setDoInput(true);
                httpClient.setDoOutput(true);

                DataOutputStream wr = new DataOutputStream(
                        httpClient.getOutputStream());
                wr.writeBytes(data.toString());
                wr.flush();
                wr.close();
            }

            if (httpClient.getResponseCode() != 200)
                throw new Exception("Invalid request");

            BufferedReader in = new BufferedReader(new InputStreamReader(httpClient.getInputStream()));

            StringBuilder response = new StringBuilder();
            String line;
            while ((line = in.readLine()) != null)
                response.append(line);

            jsonObject = new JSONObject(response.toString());
        } catch (Exception e) {
            e.printStackTrace();
        }

        return jsonObject;
    }

}
