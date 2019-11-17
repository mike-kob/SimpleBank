package api;

import sessions.Session;

public interface CardAPIINterface {

    double getBalance(Session session);

    boolean changePin(Session session, String newPin);

    Integer withdrawCash(Session session, int amount);

    boolean confirmWithdrawal(Session session, Integer txnId, boolean success, int amount);

    boolean exists(Session session, String cardNum);

    boolean transfer(Session session, String recipientCardNum, int amount);

}
