package views;

import jdk.nashorn.internal.scripts.JO;
import sessions.Session;
import utils.Constatns;
import utils.LocationHelper;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.util.HashMap;

public class ReadCardView implements View {

    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;
    private final Session session;

    private JButton proceed;

    public ReadCardView(Session session, JLayeredPane jp, HashMap<String, ActionListener> listeners) {
        this.jpane = jp;
        this.listeners = listeners;
        this.session = session;
    }

    @Override
    public void init() {
        int winY = jpane.getHeight();

        JLabel caption = new JLabel(Constatns.CARD_ENTER_TEXT);
        caption.setFont(Constatns.TITLE_FONT);
        caption.setHorizontalAlignment(SwingConstants.CENTER);
        caption.setSize(600, 100);
        int cx = (jpane.getWidth() - caption.getWidth()) / 2;
        int cy =(int) (0.1 * winY);
        caption.setLocation(cx, cy);
        caption.setVisible(true);
        jpane.add(caption, 0);

        JTextField carnNum = new JTextField();
        carnNum.setHorizontalAlignment(SwingConstants.CENTER);
        carnNum.setFont(Constatns.TITLE_FONT);
        carnNum.setSize(650, 70);
        carnNum.setLocation(LocationHelper.centerLocation(jpane, carnNum));
        carnNum.setVisible(true);
        carnNum.setToolTipText("Card number");
        carnNum.getText();
        carnNum.addKeyListener(la);
        jpane.add(carnNum, 0);

        proceed = new JButton("Enter");
        proceed.setSize(160, 80);
        proceed.setFont(new Font("Arial", Font.PLAIN, 20));
        int px = (jpane.getWidth() - proceed.getWidth()) / 2;
        int py =(int) (0.7 * winY);
        proceed.setLocation(px, py);
        proceed.setVisible(true);
        proceed.addActionListener(listeners.get("proceed_enter_card_button"));
        proceed.addActionListener(e -> session.setCardNum(carnNum.getText()));
        jpane.add(proceed, 0);

        jpane.repaint();

        carnNum.requestFocus();


    }

    private KeyListener la = new KeyListener() {
        @Override
        public void keyTyped(KeyEvent e) {
        }

        @Override
        public void keyPressed(KeyEvent e) {
            if (e.getKeyCode() == KeyEvent.VK_ENTER) {
                proceed.doClick();
            }
        }

        @Override
        public void keyReleased(KeyEvent e) {
        }
    };

    @Override
    public void disposeView() {
        jpane.removeAll();
        jpane.repaint();
    }
}
