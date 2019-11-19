package utils;

import org.json.JSONObject;
import sessions.Session;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.logging.Logger;

public class HttpHelper {
    private static Logger logger = Logger.getLogger(HttpHelper.class.getName());

    public static JSONObject Get(String targetURL, Session session) {
        return doRequest(targetURL, "GET", null, session);
    }

    public static JSONObject Post(String targetURL, JSONObject data, Session session)
    {
        return doRequest(targetURL, "POST", data, session);
    }

    public static JSONObject Put(String targetURL, JSONObject data, Session session)
    {
        return doRequest(targetURL, "PUT", data, session);
    }

    private static JSONObject doRequest(String targetURL, String method, JSONObject data, Session session)
    {
        JSONObject jsonObject = null;

        try {
            URL url = new URL(targetURL);
            HttpURLConnection httpClient = (HttpURLConnection) url.openConnection();
            httpClient.setRequestMethod(method);
            httpClient.setRequestProperty("Content-Type", "Application/json");
            httpClient.setRequestProperty("Accept-Type", "Application/json");
            if (session != null) {
                String auth = session.getAuthorization();
                if (auth != null)
                    httpClient.setRequestProperty("Authorization", auth);
            }
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
            logger.warning("" + e.getMessage());
        }

        return jsonObject;
    }

}
