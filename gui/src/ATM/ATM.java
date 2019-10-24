package ATM;

import sessions.Session;

import java.io.IOException;

public class ATM implements ATMInterface {

    public void checkoutUnits(Session session, int units) {
        if (units % 50 != 0){
            throw new IllegalArgumentException("Only 50x amount can be withdrawn");
        }

        int n_100 = units / 100;
        int n_50 = (units - n_100 * 100) / 50;

        giveOut100(n_100);
        giveOut50(n_50);

    }

    private void giveOut100(int n)
    {
        ExecMove(10, 10, 10);

    }

    private void giveOut50(int n)
    {
        ExecMove(10, 10, 10);
    }

    private Boolean ExecMove(int left, int right, int seconds)
    {
        try {
            Runtime rt = Runtime.getRuntime();
            Process pr = rt.exec("python execute.py move 30 30 5");
            pr.waitFor();
            return true;
        } catch (IOException | InterruptedException e) {
            e.printStackTrace();
            return false;
        }
    }

    public int getUnitsLeft(Session session) {
        return 0;
    }

    public Boolean isCheckoutSuccessful(Session session) {
        return null;
    }
}
