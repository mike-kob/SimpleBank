package views;

import sessions.Session;
import utils.Constatns;

import javax.swing.*;
import javax.swing.border.Border;
import java.awt.*;
import java.awt.event.ActionListener;
import java.text.ParseException;
import java.util.HashMap;

public class TransferView implements View{
    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;
    private final Session session;

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

        JTextField tfSum = new JTextField();
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

        JTextField tfCardNum = new JTextField();

        tfCardNum.setEditable(true);
        //tfCardNum.setBackground(null);
        tfCardNum.setHorizontalAlignment(SwingConstants.CENTER);
        tfCardNum.setBorder(BorderFactory.createLineBorder(Color.LIGHT_GRAY));
        tfCardNum.setFont(new Font("Arial", Font.PLAIN, 40));
        //tfCardNum.setText("moneeeeyyy");
        tfCardNum.setSize(500, 50);
        tfCardNum.setLocation((jpane.getWidth() - tfCardNum.getWidth()) / 2,lCardNum.getY() + lCardNum.getHeight() + 10);
        jpane.add(tfCardNum);

//        addButtons();

        JButton confirm = new JButton("Confirm");
        confirm.setSize(160, 80);
        confirm.setFont(new Font("Arial", Font.PLAIN, 20));
        int px = (jpane.getWidth() - confirm.getWidth()) / 2 - 130;
        confirm.setLocation(px, (int) (jpane.getHeight() * 0.8));
        confirm.setVisible(true);
        confirm.addActionListener(e -> {
            try {
                int amount = Integer.parseInt(tfSum.getText());
                String receiverNum = tfCardNum.getText();
                if (this.session.getCardAPIClient().exists(this.session, receiverNum)) {
                    boolean successful = this.session.getCardAPIClient().transfer(this.session, receiverNum, amount);
                    if (successful) {
                        JOptionPane.showMessageDialog(jpane, "Transfer successful!");
                        this.session.goToPin();
                    } else {
                        JOptionPane.showMessageDialog(jpane, "Unable to perform the trasfer!");
                    }
                } else  {
                    JOptionPane.showMessageDialog(jpane, "The card doesn't exist!");
                }

            }
            catch (NumberFormatException pe) {
                JOptionPane.showMessageDialog(jpane, "The amount is not valid");
            }

        });
        jpane.add(confirm, 0);

        JButton cancel = new JButton("Cancel");
        cancel.setSize(160, 80);
        cancel.setFont(new Font("Arial", Font.PLAIN, 20));
        cancel.setLocation(px + 250, (int) (jpane.getHeight() * 0.8));
        cancel.setVisible(true);
        cancel.addActionListener(listeners.get("cancel_button"));
        jpane.add(cancel, 0);
        jpane.repaint();
    }

    @Override
    public void disposeView() {
        jpane.removeAll();
        jpane.repaint();
    }

    private void addButtons()
    {
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
        five.setBounds(px + 115, 515,90, 90);
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
        zero.setBounds(px + 115, 745,90, 90);
        zero.setFont(new Font("Arial", Font.PLAIN, 50));
        zero.setVisible(true);
        zero.addActionListener(listeners.get("zero_button"));
        jpane.add(zero, 0);
    }
}

