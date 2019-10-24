package api;

public interface CardAPIINterface {

    double getBalance();

    void changePin(String newPin);

    void withdrawCash(int amount);

    void confirmWithdrawal(String txnId);

    boolean exists(String cardNum);

    void transfer(String recepientCardNum);

}
