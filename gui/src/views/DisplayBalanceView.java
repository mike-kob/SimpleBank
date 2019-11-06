package views;

import sessions.Session;
import utils.Constatns;
import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionListener;
import java.util.HashMap;

public class DisplayBalanceView implements View{
    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;
    private final Session session;

    public DisplayBalanceView(Session session, JLayeredPane jp, HashMap<String, ActionListener> listeners) {
        this.jpane = jp;
        this.listeners = listeners;
        this.session = session;
    }

    @Override
    public void init() {
        double balance = this.session.getCardAPIClient().getBalance(this.session);
        JLabel lBalance = new JLabel("Your balance");
        lBalance.setFont(new Font("Arial", Font.PLAIN, 100));
        lBalance.setHorizontalAlignment(SwingConstants.CENTER);
        lBalance.setSize(700, 110);
        int cx = (jpane.getWidth() - lBalance.getWidth()) / 2;
        int cy = lBalance.getHeight() + 40;
        lBalance.setLocation(cx, cy);
        lBalance.setVisible(true);
        jpane.add(lBalance);

        JLabel lYourMoney = new JLabel("Your money:" + balance);
        lYourMoney.setFont(new Font("Arial", Font.PLAIN, 60));
        lYourMoney.setHorizontalAlignment(SwingConstants.CENTER);
        lYourMoney.setBounds(300, lBalance.getY() + lBalance.getHeight() + 60, 700, 70);
        lYourMoney.setVisible(true);
        jpane.add(lYourMoney);

        JLabel lCreditLimit = new JLabel("Available credit limit:");
        lCreditLimit.setFont(new Font("Arial", Font.PLAIN, 60));
        lCreditLimit.setHorizontalAlignment(SwingConstants.CENTER);
        lCreditLimit.setBounds(400, lYourMoney.getY() + lYourMoney.getHeight() + 40, 700, 70);
        lCreditLimit.setVisible(true);
        jpane.add(lCreditLimit);

        JButton goBack = new JButton("Go back");
        goBack.setSize(160, 80);
        goBack.setFont(new Font("Arial", Font.PLAIN, 20));
        goBack.setLocation(480, lCreditLimit.getY() + lCreditLimit.getHeight() + 60);
        goBack.setVisible(true);
        goBack.addActionListener(listeners.get("cancel_button"));
        jpane.add(goBack, 0);
        jpane.repaint();
    }

    @Override
    public void disposeView() {
        jpane.removeAll();
        jpane.repaint();
    }
}

