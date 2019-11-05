package api;

import org.json.JSONObject;
import sessions.Session;
import utils.Constatns;
import utils.HttpHelper;

public class UserAPI implements UserAPIInterface {

    public Session startSession() {
        JSONObject data = new JSONObject();
        JSONObject response = HttpHelper.Post(Constatns.START_SESSION_URL, data);
        Session session = new Session();

        if (response != null && response.getBoolean("Ok")){
            return session;
        } else {
            return null;
        }
    }

    public boolean login(Session session) {
        JSONObject data = new JSONObject();
        data.put("card_num", session.getCardNum());
        data.put("PIN", session.getCardPin());
        JSONObject response = HttpHelper.Post(Constatns.LOGIN_URL, data);

        return  (response != null && response.getBoolean("Ok"));
    }
}
