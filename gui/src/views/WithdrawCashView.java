package views;

import sessions.Session;
import utils.Constatns;
import utils.LocationHelper;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.util.HashMap;

public class WithdrawCashView implements View{
    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;
    private final Session session;

    private JButton cancel, confirm;

    public WithdrawCashView(Session session, JLayeredPane jp, HashMap<String, ActionListener> listeners) {
        this.jpane = jp;
        this.listeners = listeners;
        this.session = session;
    }

    @Override
    public void init() {
        int winY = jpane.getHeight();

        JLabel caption = new JLabel("Enter sum you want to withdraw:");
        caption.setFont(new Font("Arial", Font.PLAIN, 40));
        caption.setHorizontalAlignment(SwingConstants.CENTER);
        caption.setSize(1000, 50);
        int cx = (jpane.getWidth() - caption.getWidth()) / 2;
        int cy = (int) (winY * 0.1);
        caption.setLocation(cx, cy);
        caption.setVisible(true);
        jpane.add(caption);

        JTextField tfSum = new JTextField();
        tfSum.setEditable(true);
        tfSum.setBackground(Color.white);
        tfSum.setHorizontalAlignment(SwingConstants.CENTER);
        tfSum.setBorder(BorderFactory.createLineBorder(Color.LIGHT_GRAY));
        tfSum.setFont(new Font("Arial", Font.PLAIN, 80));
        //tfSum.setHorizontalAlignment(SwingConstants.CENTER);
        //tfSum.setText("moneeeeyyy");
        tfSum.setSize(600, 100);
        tfSum.setLocation(LocationHelper.centerLocation(jpane, tfSum));
        jpane.add(tfSum);

        confirm = new JButton("Confirm");
        confirm.setSize(160, 80);
        confirm.setFont(new Font("Arial", Font.PLAIN, 20));
        int px = (jpane.getWidth() - confirm.getWidth()) / 2 - 130;
        confirm.setLocation(px, (int) (winY * 0.8));
        confirm.setVisible(true);
        confirm.addActionListener(e -> {
            try {
                int amount = Integer.parseInt(tfSum.getText());
                boolean accepted = this.session.getCardAPIClient().withdrawCash(this.session, amount);
                if (accepted) {
                    JOptionPane.showMessageDialog(jpane, "Take your money");
                    this.session.getaATMClient().checkoutUnits(this.session, 100);
                    this.session.goToPin();
                } else  {
                    JOptionPane.showMessageDialog(jpane, "Unable to perform the operation!");
                }

            } catch (NumberFormatException f) {
                JOptionPane.showMessageDialog(jpane, "Invalid amount.");
            }
        });
        jpane.add(confirm, 0);

        cancel = new JButton("Cancel");
        cancel.setSize(160, 80);
        cancel.setFont(new Font("Arial", Font.PLAIN, 20));
        cancel.setLocation(px + 250, (int) (winY * 0.8));
        cancel.setVisible(true);
        cancel.addActionListener(listeners.get("cancel_button"));
        jpane.add(cancel, 0);
        jpane.repaint();

        tfSum.requestFocus();
        tfSum.addKeyListener(keyListener);
    }

    @Override
    public void disposeView() {
        jpane.removeAll();
        jpane.repaint();
    }

    private KeyListener keyListener = new KeyListener() {
        @Override
        public void keyTyped(KeyEvent e) {

        }

        @Override
        public void keyPressed(KeyEvent e) {
            if (e.getKeyCode() == KeyEvent.VK_ENTER) {
                confirm.doClick();
            } else if (e.getKeyCode() == Constatns.CANCEL_KEY) {
                cancel.doClick();
            }
        }

        @Override
        public void keyReleased(KeyEvent e) {

        }
    };

}

