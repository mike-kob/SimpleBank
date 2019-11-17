package views;

import sessions.Session;
import utils.Constatns;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.util.HashMap;

public class ChangePinView implements View {
    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;
    private final Session session;


    private JPasswordField pinFieldOld;
    private JPasswordField pinFieldNew;

    private JButton confirm;
    private JButton cancel;

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

    public ChangePinView(Session session, JLayeredPane jp, HashMap<String, ActionListener> listeners) {
        this.jpane = jp;
        this.listeners = listeners;
        this.session = session;
    }

    @Override
    public void init() {
        int winX = jpane.getHeight();

        JLabel captionOld = new JLabel("Enter new PIN-code:");
        captionOld.setFont(Constatns.TITLE_FONT);
        captionOld.setHorizontalAlignment(SwingConstants.CENTER);
        captionOld.setSize(700, 40);
        int cx = (jpane.getWidth() - captionOld.getWidth()) / 2;
        int cy = (int) (winX * 0.1);
        captionOld.setLocation(cx - 10, cy);
        captionOld.setVisible(true);
        jpane.add(captionOld);

        pinFieldOld = new JPasswordField();
        pinFieldOld.setSize(250, 50);
        pinFieldOld.setLocation((jpane.getWidth() - pinFieldOld.getWidth()) / 2, cy + pinFieldOld.getHeight() + 10);
        pinFieldOld.setFont(new Font("Arial", Font.PLAIN, 40));
        pinFieldOld.setBorder(BorderFactory.createLineBorder(Color.LIGHT_GRAY));
        pinFieldOld.setHorizontalAlignment(SwingConstants.CENTER);

        JLabel captionNew = new JLabel("Re-enter the PIN-code:");
        captionNew.setFont(Constatns.TITLE_FONT);
        captionNew.setBounds(cx, pinFieldOld.getHeight() + pinFieldOld.getY() + 20, 700, 40);
        captionNew.setVisible(true);
        captionNew.setHorizontalAlignment(SwingConstants.CENTER);
        jpane.add(captionNew);

        pinFieldNew = new JPasswordField();
        pinFieldNew.setFont(new Font("Arial", Font.PLAIN, 40));
        pinFieldNew.setHorizontalAlignment(SwingConstants.CENTER);
        pinFieldNew.setBorder(BorderFactory.createLineBorder(Color.LIGHT_GRAY));
        pinFieldNew.setBounds((jpane.getWidth() - pinFieldOld.getWidth()) / 2, captionNew.getY() + captionNew.getHeight() + 10, 250, 50);


        pinFieldOld.addKeyListener(new KeyListener() {
            @Override
            public void keyTyped(KeyEvent e) {

            }

            @Override
            public void keyPressed(KeyEvent e) {
                if (e.getKeyCode() == KeyEvent.VK_ENTER) {
                    pinFieldNew.requestFocus();
                } else if (e.getKeyCode() == Constatns.CANCEL_KEY) {
                    cancel.doClick();
                }
            }

            @Override
            public void keyReleased(KeyEvent e) {

            }
        });
        pinFieldNew.addKeyListener(new KeyListener() {
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

        jpane.add(pinFieldNew, 0);
        jpane.add(pinFieldOld, 0);

        pinFieldOld.requestFocus();
        pinFieldOld.setText("");

//        addButtons();


        confirm = new JButton("Confirm");
        confirm.setSize(160, 80);
        confirm.setFont(new Font("Arial", Font.PLAIN, 20));
        int px = (jpane.getWidth() - confirm.getWidth()) / 2 - 130;
        confirm.setLocation(px, (int) (winX * 0.8));
        confirm.setVisible(true);
        confirm.addActionListener(e -> {
            if (!pinFieldOld.getText().equals(pinFieldNew.getText())) {
                JOptionPane.showMessageDialog(jpane, "PIN-codes don't match");
                pinFieldOld.setText("");
                pinFieldNew.setText("");
                pinFieldOld.requestFocus();
            } else {
                boolean succes = this.session.getCardAPIClient().changePin(this.session, pinFieldNew.getText());
                if (!succes)
                    JOptionPane.showMessageDialog(jpane, "Error occurred");
                else {
                    JOptionPane.showMessageDialog(jpane, "PIN-code changed successfully");
                    this.session.goToPin();
                }
            }
        });
        jpane.add(confirm, 0);

        cancel = new JButton("Cancel");
        cancel.setSize(160, 80);
        cancel.setFont(Constatns.BUTTON_FONT);
        cancel.setLocation(px + 250, (int) (winX * 0.8));
        cancel.setVisible(true);
        cancel.addActionListener(listeners.get("cancel_button"));
        jpane.add(cancel, 0);
        jpane.repaint();
    }

    @Override
    public void disposeView() {
        jpane.removeKeyListener(keyListener);
        jpane.removeAll();
        jpane.repaint();
    }

    private void addButtons() {
        JButton one = new JButton("1");
        one.setSize(90, 90);
        one.setFont(new Font("Arial", Font.PLAIN, 50));
        int px = (jpane.getWidth() - one.getWidth()) / 2 - 120;
        one.setLocation(px, 400);
        one.setVisible(true);
        one.addActionListener(listeners.get("one_button"));
        jpane.add(one, 0);

        JButton two = new JButton("2");
        two.setSize(90, 90);
        two.setFont(new Font("Arial", Font.PLAIN, 50));
        two.setLocation(px + 115, 400);
        two.setVisible(true);
        two.addActionListener(listeners.get("two_button"));
        jpane.add(two, 0);

        JButton three = new JButton("3");
        three.setSize(90, 90);
        three.setFont(new Font("Arial", Font.PLAIN, 50));
        three.setLocation(px + 230, 400);
        three.setVisible(true);
        three.addActionListener(listeners.get("three_button"));
        jpane.add(three, 0);

        JButton four = new JButton("4");
        four.setSize(90, 90);
        four.setFont(new Font("Arial", Font.PLAIN, 50));
        four.setLocation(px, 515);
        four.setVisible(true);
        four.addActionListener(listeners.get("four_button"));
        jpane.add(four, 0);

        JButton five = new JButton("5");
        five.setBounds(px + 115, 515, 90, 90);
        five.setFont(new Font("Arial", Font.PLAIN, 50));
        five.setVisible(true);
        five.addActionListener(listeners.get("five_button"));
        jpane.add(five, 0);

        JButton six = new JButton("6");
        six.setBounds(px + 230, 515, 90, 90);
        six.setFont(new Font("Arial", Font.PLAIN, 50));
        six.setVisible(true);
        six.addActionListener(listeners.get("six_button"));
        jpane.add(six, 0);

        JButton seven = new JButton("7");
        seven.setBounds(px, 630, 90, 90);
        seven.setFont(new Font("Arial", Font.PLAIN, 50));
        seven.setLocation(px, 630);
        seven.setVisible(true);
        seven.addActionListener(listeners.get("seven_button"));
        jpane.add(seven, 0);

        JButton eight = new JButton("8");
        eight.setBounds(px + 115, 630, 90, 90);
        eight.setFont(new Font("Arial", Font.PLAIN, 50));
        eight.setVisible(true);
        eight.addActionListener(listeners.get("eight_button"));
        jpane.add(eight, 0);

        JButton nine = new JButton("9");
        nine.setBounds(px + 230, 630, 90, 90);
        nine.setFont(new Font("Arial", Font.PLAIN, 50));
        nine.setVisible(true);
        nine.addActionListener(listeners.get("nine_button"));
        jpane.add(nine, 0);

        JButton zero = new JButton("0");
        zero.setBounds(px + 115, 745, 90, 90);
        zero.setFont(new Font("Arial", Font.PLAIN, 50));
        zero.setVisible(true);
        zero.addActionListener(listeners.get("zero_button"));
        jpane.add(zero, 0);
    }
}

