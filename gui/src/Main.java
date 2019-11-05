import ATM.ATM;
import org.json.JSONObject;
import sessions.Session;
import sessions.SessionManager;
import utils.HttpHelper;

import javax.swing.*;

public class Main {
    private static final JFrame frame = new JFrame();
    private static final JLayeredPane lp = frame.getLayeredPane();

    public static void main(String[] args) {
        initWindow();
        SessionManager.Initialize(lp);
    }

    private static void initWindow() {
        frame.setExtendedState(JFrame.MAXIMIZED_BOTH);
        frame.setVisible(true);
        frame.setLayout(null);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setResizable(true);
    }
}
