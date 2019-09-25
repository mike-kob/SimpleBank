package api;

import sessions.Session;

public class CardAPI implements CardAPIINterface {

    public CardAPI(Session session) {

    }

    public double getBalance() {
        return 0;
    }

    public void changePin(String newPin) {

    }

    public void withdrawCash(int amount) {

    }

    public void confirmWithdrawal(String txnId) {

    }

    public boolean exists(String cardNum) {
        return false;
    }

    public void transfer(String recepientCardNum) {

    }
}
