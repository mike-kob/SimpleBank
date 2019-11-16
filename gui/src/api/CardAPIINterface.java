package api;

import sessions.Session;

public interface CardAPIINterface {

    double getBalance(Session session);

    boolean changePin(Session session, String newPin);

    boolean withdrawCash(Session session, int amount);

    boolean confirmWithdrawal(Session session);

    boolean exists(Session session, String cardNum);

    boolean transfer(Session session, String recipientCardNum, int amount);

}
