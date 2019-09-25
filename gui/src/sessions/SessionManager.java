package sessions;

import utils.LocationHelper;

import javax.swing.*;

public class SessionManager {

    private static Session currentSession = null;
    private static JLayeredPane jpane = null;

    public static void Initialize(JLayeredPane jp) {
        jpane = jp;
        initSleepWindow();
    }

    private static void initSleepWindow(){
        JButton start = new JButton("Start");
        start.setSize(160, 80);
        start.setVisible(true);
        start.setLocation(LocationHelper.centerLocation(jpane, start));
        start.addActionListener(e -> {
            startSession();
        });
        jpane.add(start,0);
    }

    private static void startSession(){
        jpane.removeAll();
        jpane.repaint();
        currentSession = new Session(jpane);
    }


}
