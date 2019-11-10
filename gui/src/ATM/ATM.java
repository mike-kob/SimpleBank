package ATM;

import sessions.Session;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.util.concurrent.TimeUnit;

public class ATM implements ATMInterface {

    public boolean checkoutUnits(Session session, int units) {
        try {
            Process p = Runtime.getRuntime().exec("python bin/execute.py move 50 50 3");
            BufferedReader in = new BufferedReader(new InputStreamReader(p.getInputStream()));
            String ret = in.readLine();
            boolean a = p.waitFor(10, TimeUnit.SECONDS);
            return a;
        } catch (Exception a) {
            a.printStackTrace();
            return false;
        }
    }
}
