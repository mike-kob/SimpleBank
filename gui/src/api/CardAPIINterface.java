package api;

import org.json.JSONObject;
import sessions.Session;

import java.util.Map;

public interface CardAPIINterface {

    Map<String, Double> getBalance(Session session);

    boolean changePin(Session session, String newPin);

    JSONObject withdrawCash(Session session, int amount);

    boolean confirmWithdrawal(Session session, Integer txnId, boolean success, int amount);

    boolean exists(Session session, String cardNum);

    JSONObject transfer(Session session, String recipientCardNum, int amount);

}
