package views;

import utils.Constatns;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionListener;
import java.util.HashMap;

public class OptionsView implements View {
    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;

    public OptionsView(JLayeredPane jp, HashMap<String, ActionListener> listeners) {
        this.jpane = jp;
        this.listeners = listeners;
    }

    @Override
    public void init() {
        JLabel caption = new JLabel("Please, select an option:");
        caption.setFont(Constatns.TITLE_FONT);
        caption.setHorizontalAlignment(SwingConstants.CENTER);
        caption.setSize(700, 400);
        int cx = (jpane.getWidth() - caption.getWidth()) / 2;
        caption.setLocation(cx, 30);
        caption.setVisible(true);
        jpane.add(caption, 0);

        addOptions();

        jpane.repaint();
    }

    @Override
    public void disposeView() {
        jpane.removeAll();
        jpane.repaint();
    }

    private void addOptions(){
        JButton changePin = new JButton("Change PIN");
        changePin.setSize(360, 100);
        changePin.setFont(new Font("Arial", Font.PLAIN, 20));
        int px = (jpane.getWidth() - changePin.getWidth()) / 2 - 300;
        changePin.setLocation(px, 320);
        changePin.setVisible(true);
        changePin.addActionListener(listeners.get("change_pin_button"));
        jpane.add(changePin, 0);

        JButton withdrawCash = new JButton("Withdraw cash");
        withdrawCash.setFont(new Font("Arial", Font.PLAIN, 20));
        withdrawCash.setBounds(px + 550, 320, 360, 100);
        withdrawCash.setVisible(true);
        withdrawCash.addActionListener(listeners.get("withdraw_cash_button"));
        jpane.add(withdrawCash, 0);

        JButton viewBalance = new JButton("View balance");
        viewBalance.setFont(new Font("Arial", Font.PLAIN, 20));
        viewBalance.setBounds(px, 480, 360, 100);
        viewBalance.setVisible(true);
        viewBalance.addActionListener(listeners.get("view_balance_button"));
        jpane.add(viewBalance, 0);

        JButton transfer = new JButton("Transfer money");
        transfer.setFont(new Font("Arial", Font.PLAIN, 20));
        transfer.setBounds(px + 550, 480, 360, 100);
        transfer.setVisible(true);
        transfer.addActionListener(listeners.get("transfer_button"));
        jpane.add(transfer, 0);

        JButton finish = new JButton("Finish");
        finish.setFont(new Font("Arial", Font.PLAIN, 20));
        finish.setBounds(px + 270, 640,360, 100);
        finish.setVisible(true);
        finish.addActionListener(listeners.get("finish_session"));
        jpane.add(finish, 0);
    }
}
