package api;

import org.json.JSONObject;
import sessions.Session;
import utils.Constatns;
import utils.HttpHelper;

public class AtmAPI implements AtmAPIInterface {

    public int getRemaining(Session session) {
        JSONObject response = HttpHelper.Get(Constatns.HOST + Constatns.ATM_REMAINING_URL, session);
        if  (response != null && response.getBoolean("ok"))
        {
            return response.getInt("remaining");
        }
        else
        {
            return 0;
        }
    }

    public void withdraw(Session session, int units) {
        JSONObject data = new JSONObject();
        data.put("atmId", 1);
        data.put("amount", units);

        HttpHelper.Post(Constatns.HOST + Constatns.ATM_WITHDRAW_URL, data, session);
    }
}
