package views;

import utils.Constatns;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.util.HashMap;

public class OptionsView implements View {
    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;

    private JButton changePin, withdrawCash, viewBalance, transfer, finish;

    private KeyListener keyListener = new KeyListener() {
        @Override
        public void keyTyped(KeyEvent e) {

        }

        @Override
        public void keyPressed(KeyEvent e) {
            if (e.getKeyCode() == KeyEvent.VK_NUMPAD1) {
                changePin.doClick();
            } else if (e.getKeyCode() == KeyEvent.VK_NUMPAD2) {
                withdrawCash.doClick();
            } else if (e.getKeyCode() == KeyEvent.VK_NUMPAD3) {
                viewBalance.doClick();
            } else if (e.getKeyCode() == KeyEvent.VK_NUMPAD4) {
                transfer.doClick();
            } else if (e.getKeyCode() == Constatns.CANCEL_KEY) {
                finish.doClick();
            }


        }

        @Override
        public void keyReleased(KeyEvent e) {

        }
    };

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
        jpane.removeKeyListener(keyListener);
        jpane.removeAll();
        jpane.repaint();
    }

    private void addOptions() {
        int winY = jpane.getHeight();
        Dimension bSize = new Dimension(300, 70);

        changePin = new JButton("1: Change PIN");
        changePin.setSize(bSize);
        changePin.setFont(new Font("Arial", Font.PLAIN, 20));
        int px = 0;
        changePin.setLocation(px, (int) (winY * 0.3));
        changePin.setVisible(true);
        changePin.addActionListener(listeners.get("change_pin_button"));
        jpane.add(changePin, 0);

        withdrawCash = new JButton("2: Withdraw cash");
        withdrawCash.setFont(new Font("Arial", Font.PLAIN, 20));
        withdrawCash.setFont(new Font("Arial", Font.PLAIN, 20));
        withdrawCash.setSize(bSize);
        withdrawCash.setLocation(jpane.getWidth() - withdrawCash.getWidth(), (int) (winY * 0.3));
        withdrawCash.setVisible(true);
        if (Constatns.VIRTUAL) {
            withdrawCash.setToolTipText("Sorry, withdrawal doesn't work on virtual ATM");
            withdrawCash.setEnabled(false);
        } else {
            withdrawCash.addActionListener(listeners.get("withdraw_cash_button"));
        }
        jpane.add(withdrawCash, 0);

        viewBalance = new JButton("3: View balance");
        viewBalance.setFont(new Font("Arial", Font.PLAIN, 20));
        viewBalance.setSize(bSize);
        viewBalance.setLocation(px, (int) (winY * 0.6));
        viewBalance.setVisible(true);
        viewBalance.addActionListener(listeners.get("view_balance_button"));
        jpane.add(viewBalance, 0);

        transfer = new JButton("4: Transfer money");
        transfer.setFont(new Font("Arial", Font.PLAIN, 20));
        transfer.setSize(bSize);
        transfer.setLocation(jpane.getWidth() - transfer.getWidth(), (int) (winY * 0.6));
        transfer.setVisible(true);
        transfer.addActionListener(listeners.get("transfer_button"));
        jpane.add(transfer, 0);

        finish = new JButton("Finish");
        finish.setFont(new Font("Arial", Font.PLAIN, 20));
        finish.setSize(bSize);
        finish.setLocation((jpane.getWidth() - finish.getWidth()) / 2, (int) (winY * 0.8));
        finish.setVisible(true);
        finish.addActionListener(listeners.get("finish_session"));
        jpane.add(finish, 0);
        jpane.requestFocus();
        jpane.addKeyListener(keyListener);
    }
}
