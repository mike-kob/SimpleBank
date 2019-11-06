package sessions;

import api.CardAPI;
import api.CardAPIINterface;
import api.UserAPI;
import api.UserAPIInterface;
import utils.LocationHelper;

import javax.swing.*;
import java.awt.*;

public class SessionManager {

    private static CardAPIINterface cardAPI = new CardAPI();
    private static UserAPIInterface userAPI = new UserAPI();

    private static Session currentSession = null;
    private static JLayeredPane jpane = null;

    public static void Initialize(JLayeredPane jp) {
        jpane = jp;
        initSleepWindow();
    }

    private static void initSleepWindow(){
        JButton start = new JButton("Start");
        start.setSize(160, 80);
        start.setFont(new Font("Arial", Font.PLAIN, 20));
        start.setVisible(true);
        start.setLocation(LocationHelper.centerLocation(jpane, start));
        start.addActionListener(e -> {
            Session session = userAPI.startSession();
            if (session != null)
                startSession(session);
            else
                JOptionPane.showMessageDialog(jpane, "Sorry, ATM is currently not working");
        });

        jpane.add(start, 0);

    }

    private static void startSession(Session session){
        jpane.removeAll();
        jpane.repaint();
        session.setJpane(jpane);
        currentSession = session;
        currentSession.show();
    }

    static void finishSession(){
        currentSession = null;
        jpane.removeAll();
        jpane.repaint();
        initSleepWindow();
    }


}
