import sys

from Bot import Bot

bot = Bot()

if __name__ == '__main__':
    command = sys.argv[1]
    if command == 'move':
        if len(sys.argv) < 4:
            raise ValueError('Incorrect usage. \n'
                             'python execute.py move LEFT_WHEEL RIGHT WHEEL DURATION')
        left = int(sys.argv[2])
        right = int(sys.argv[3])
        duration = int(sys.argv[4])

        bot.move(left, right, duration)
    elif command == 'startcamera':
        bot.start_camera()
