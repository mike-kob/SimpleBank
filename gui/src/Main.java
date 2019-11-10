import jdk.nashorn.internal.scripts.JO;
import sessions.SessionManager;
import utils.Constatns;

import javax.swing.*;
import java.awt.event.ComponentAdapter;
import java.awt.event.ComponentEvent;
import java.io.*;

public class Main {
    private static final JFrame frame = new JFrame();
    private static final JLayeredPane lp = frame.getLayeredPane();

    public static void main(String[] args) {
        String path = "./bin/properties.txt";
        FileInputStream fis = null;
        try {
            fis = new FileInputStream(path);
            BufferedReader in = new BufferedReader(new InputStreamReader(fis));
            String host = in.readLine();
            Constatns.HOST = host;
            Constatns.VIRTUAL = Boolean.parseBoolean(in.readLine());

        } catch (IOException e) {
            e.printStackTrace();
        }
        initWindow();
        SessionManager.Initialize(lp);

    }

    private static void initWindow() {
        frame.setSize(720, 480);
        if (Constatns.VIRTUAL)
            frame.setExtendedState(JFrame.MAXIMIZED_BOTH);
        frame.setVisible(true);
        frame.setLayout(null);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setResizable(true);
    }
}
