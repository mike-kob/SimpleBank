package sessions;

import api.CardAPI;
import api.CardAPIINterface;
import api.UserAPI;
import api.UserAPIInterface;
import utils.Constatns;
import utils.InactivityListener;
import utils.LocationHelper;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.concurrent.TimeUnit;

public class SessionManager {

    private static CardAPIINterface cardAPI = new CardAPI();
    private static UserAPIInterface userAPI = new UserAPI();

    private static Session currentSession = null;
    private static JLayeredPane jpane = null;
    private static JFrame frame = null;

    private static JButton start;
    private static InactivityListener listener;

    public static void Initialize(JLayeredPane jp, JFrame fr) {
        jpane = jp;
        frame = fr;
        initSleepWindow();
    }

    private static void initSleepWindow(){
        start = new JButton("Start");
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
        jpane.requestFocus();
        jpane.addKeyListener(keyListener);

        jpane.add(start, 0);

    }

    private static KeyListener keyListener = new KeyListener() {
        @Override
        public void keyTyped(KeyEvent e) {

        }

        @Override
        public void keyPressed(KeyEvent e) {
            start.doClick();
        }

        @Override
        public void keyReleased(KeyEvent e) {

        }
    };

    private static void startSession(Session session){
        jpane.removeAll();
        jpane.removeKeyListener(keyListener);
        jpane.repaint();
        session.setJpane(jpane);
        currentSession = session;
        currentSession.show();
        listener = new InactivityListener(frame, new AbstractAction()
        {
            public void actionPerformed(ActionEvent e)
            {
                finishSession();
                JOptionPane.showMessageDialog(frame, "Your session is closed due to inactivity");
            }
        });
        listener.setIntervalInMillis(Constatns.INACTIVITY_SECONDS * 1000);
        listener.start();
    }

    static void finishSession(){
        currentSession = null;
        jpane.removeAll();
        jpane.repaint();
        listener.stop();
        initSleepWindow();
    }


}
