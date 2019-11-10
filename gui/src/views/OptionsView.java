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
        caption.setSize(700, 50);
        int cx = (jpane.getWidth() - caption.getWidth()) / 2;
        caption.setLocation(cx, (int) (jpane.getHeight() * 0.1));
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
        int winY = jpane.getHeight();
        Dimension bSize = new Dimension(300, 70);

        JButton changePin = new JButton("Change PIN");
        changePin.setSize(bSize);
        changePin.setFont(new Font("Arial", Font.PLAIN, 20));
        int px = 0;
        changePin.setLocation(px, (int) (winY * 0.3));
        changePin.setVisible(true);
        changePin.addActionListener(listeners.get("change_pin_button"));
        jpane.add(changePin, 0);

        JButton withdrawCash = new JButton("Withdraw cash");
        withdrawCash.setFont(new Font("Arial", Font.PLAIN, 20));
        withdrawCash.setFont(new Font("Arial", Font.PLAIN, 20));
        withdrawCash.setSize(bSize);
        withdrawCash.setLocation(jpane.getWidth() - withdrawCash.getWidth(), (int) (winY * 0.3));
        withdrawCash.setVisible(true);
        if (Constatns.VIRTUAL) {
            withdrawCash.setToolTipText("Sorry, withdrawal doesn't work on virtual ATM");
            withdrawCash.setEnabled(false);
        }
        else {
            withdrawCash.addActionListener(listeners.get("withdraw_cash_button"));
        }
        jpane.add(withdrawCash, 0);

        JButton viewBalance = new JButton("View balance");
        viewBalance.setFont(new Font("Arial", Font.PLAIN, 20));
        viewBalance.setSize(bSize);
        viewBalance.setLocation(px, (int) (winY * 0.6));
        viewBalance.setVisible(true);
        viewBalance.addActionListener(listeners.get("view_balance_button"));
        jpane.add(viewBalance, 0);

        JButton transfer = new JButton("Transfer money");
        transfer.setFont(new Font("Arial", Font.PLAIN, 20));
        transfer.setSize(bSize);
        transfer.setLocation(jpane.getWidth() - transfer.getWidth(), (int) (winY * 0.6));
        transfer.setVisible(true);
        transfer.addActionListener(listeners.get("transfer_button"));
        jpane.add(transfer, 0);

        JButton finish = new JButton("Finish");
        finish.setFont(new Font("Arial", Font.PLAIN, 20));
        finish.setSize(bSize);
        finish.setLocation((jpane.getWidth() - finish.getWidth())/2, (int)(winY * 0.8));
        finish.setVisible(true);
        finish.addActionListener(listeners.get("finish_session"));
        jpane.add(finish, 0);
    }
}
