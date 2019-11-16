package views;

import sessions.Session;
import utils.Constatns;

import javax.swing.*;
import javax.swing.border.Border;
import java.awt.*;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.text.ParseException;
import java.util.HashMap;

public class TransferView implements View{
    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;
    private final Session session;

    private JTextField tfSum;
    private JTextField tfCardNum;
    private JButton confirm, cancel;

    public TransferView(Session session, JLayeredPane jp, HashMap<String, ActionListener> listeners) {
        this.jpane = jp;
        this.listeners = listeners;
        this.session = session;
    }

    @Override
    public void init() {
        JLabel lSum = new JLabel("Enter sum you want to transfer:");
        lSum.setFont(Constatns.TITLE_FONT);
        lSum.setHorizontalAlignment(SwingConstants.CENTER);
        lSum.setSize(500, 50);
        int cx = (jpane.getWidth() - lSum.getWidth()) / 2;
        int cy = lSum.getHeight() + 10;
        lSum.setLocation(cx, cy);
        lSum.setVisible(true);
        jpane.add(lSum);

        tfSum = new JTextField();
        tfSum.setEditable(true);
        tfSum.setHorizontalAlignment(SwingConstants.CENTER);
        tfSum.setBorder(BorderFactory.createLineBorder(Color.LIGHT_GRAY));
        tfSum.setFont(new Font("Arial", Font.PLAIN, 40));
        //tfSum.setText("moneeeeyyy");
        //tfSum.setBackground(null);
        tfSum.setSize(250, 50);
        tfSum.setLocation((jpane.getWidth() - tfSum.getWidth()) / 2,lSum.getY() + lSum.getHeight() + 10);
        jpane.add(tfSum);

        JLabel lCardNum = new JLabel("Enter receiver's card number:");
        lCardNum.setFont(Constatns.TITLE_FONT);
        lCardNum.setHorizontalAlignment(SwingConstants.CENTER);
        lCardNum.setSize(700, 50);
        lCardNum.setLocation((jpane.getWidth() - lCardNum.getWidth()) / 2 - 10, tfSum.getY() + tfSum.getHeight() + 20);
        lCardNum.setVisible(true);
        jpane.add(lCardNum);

        tfCardNum = new JTextField();

        tfCardNum.setEditable(true);
        //tfCardNum.setBackground(null);
        tfCardNum.setHorizontalAlignment(SwingConstants.CENTER);
        tfCardNum.setBorder(BorderFactory.createLineBorder(Color.LIGHT_GRAY));
        tfCardNum.setFont(new Font("Arial", Font.PLAIN, 40));
        //tfCardNum.setText("moneeeeyyy");
        tfCardNum.setSize(500, 50);
        tfCardNum.setLocation((jpane.getWidth() - tfCardNum.getWidth()) / 2,lCardNum.getY() + lCardNum.getHeight() + 10);
        jpane.add(tfCardNum);

        confirm = new JButton("Confirm");
        confirm.setSize(160, 80);
        confirm.setFont(new Font("Arial", Font.PLAIN, 20));
        int px = (jpane.getWidth() - confirm.getWidth()) / 2 - 130;
        confirm.setLocation(px, (int) (jpane.getHeight() * 0.8));
        confirm.setVisible(true);
        confirm.addActionListener(e -> {
            performConfirm();
        });
        jpane.add(confirm, 0);

        cancel = new JButton("Cancel");
        cancel.setSize(160, 80);
        cancel.setFont(new Font("Arial", Font.PLAIN, 20));
        cancel.setLocation(px + 250, (int) (jpane.getHeight() * 0.8));
        cancel.setVisible(true);
        cancel.addActionListener(listeners.get("cancel_button"));
        jpane.add(cancel, 0);

        tfSum.addKeyListener(new KeyListener() {
            @Override
            public void keyTyped(KeyEvent e) {

            }

            @Override
            public void keyPressed(KeyEvent e) {
                if (e.getKeyCode() == KeyEvent.VK_ENTER) {
                    tfCardNum.requestFocus();
                } else if (e.getKeyCode() == Constatns.CANCEL_KEY) {
                    cancel.doClick();
                }
            }

            @Override
            public void keyReleased(KeyEvent e) {

            }
        });
        tfCardNum.addKeyListener(new KeyListener() {
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
        });
        jpane.addKeyListener(keyListener);

        jpane.repaint();
        tfSum.requestFocus();
    }

    private void performConfirm () {
        try {
            int amount = Integer.parseInt(tfSum.getText());
            String receiverNum = tfCardNum.getText();
            if (this.session.getCardAPIClient().exists(this.session, receiverNum)) {
                boolean successful = this.session.getCardAPIClient().transfer(this.session, receiverNum, amount);
                if (successful) {
                    JOptionPane.showMessageDialog(jpane, "Transfer successful!");
                    this.session.goToPin();
                } else {
                    JOptionPane.showMessageDialog(jpane, "Unable to perform the transfer!");
                }
            } else  {
                JOptionPane.showMessageDialog(jpane, "The card doesn't exist!");
                tfCardNum.setText("");
                tfCardNum.requestFocus();
            }

        }
        catch (NumberFormatException pe) {
            JOptionPane.showMessageDialog(jpane, "The amount is not valid");
            tfSum.setText("");
            tfSum.requestFocus();
        }
    }

    @Override
    public void disposeView() {
        jpane.removeKeyListener(keyListener);
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

