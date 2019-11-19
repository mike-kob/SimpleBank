package api;

import org.json.JSONObject;
import sessions.Session;
import utils.Constatns;
import utils.HttpHelper;

import java.util.HashMap;
import java.util.Map;

public class CardAPI implements CardAPIINterface {

    public Map<String, Double> getBalance(Session session)
    {
        JSONObject response = HttpHelper.Get(Constatns.HOST + Constatns.BALANCE_URL + session.getCardNum(), session);
        if (response != null && response.getBoolean("ok")) {
            HashMap<String, Double> res = new HashMap<>();
            if (response.has("limit")) {
                res.put("balance", response.getDouble("ownMoney"));
                res.put("creditLimit", response.getDouble("limit"));
            } else {
                res.put("balance", response.getDouble("balance"));
            }

            return res;
        } else {
            return null;
        }
    }

    public boolean changePin(Session session, String newPin) {
        JSONObject data = new JSONObject();
        data.put("cardNum", session.getCardNum());
        data.put("oldPin", session.getCardPin());
        data.put("newPin", newPin);

        JSONObject response = HttpHelper.Put(Constatns.HOST + Constatns.CHANGE_PIN_URL, data, session);
        return  (response != null && response.getBoolean("ok"));
    }

    public JSONObject withdrawCash(Session session, int amount) {
        JSONObject data = new JSONObject();
        data.put("cardNum", session.getCardNum());
        data.put("amount", amount);

        return HttpHelper.Post(Constatns.HOST + Constatns.WITHDRAW_URL, data, session);
    }

    public boolean confirmWithdrawal(Session session, Integer txnId, boolean success, int amount) {
        JSONObject data = new JSONObject();
        data.put("cardNum", session.getCardNum());
        data.put("success", success);
        data.put("amount", amount);
        data.put("txnId", txnId);

        JSONObject response = HttpHelper.Post(Constatns.HOST + Constatns.CONFIRM_WITHDRAW_URL, data, session);
        return  (response != null && response.getBoolean("ok"));
    }

    public boolean exists(Session session, String cardNum) {
        JSONObject response = HttpHelper.Get(Constatns.HOST + Constatns.CARD_EXISTS_URL + session.getCardNum(), session);
        return  (response != null && response.getBoolean("ok") && response.getBoolean("isValid"));
    }

    public JSONObject transfer(Session session, String recipientCardNum, int amount) {
        JSONObject data = new JSONObject();
        data.put("cardNumFrom", session.getCardNum());
        data.put("cardNumTo", recipientCardNum);
        data.put("amount", amount);

        JSONObject response = HttpHelper.Post(Constatns.HOST + Constatns.TRANSFER_URL, data, session);
        return  response;
    }
}
