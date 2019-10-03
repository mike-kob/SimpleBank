from camera.camera import start_camera
from wheels import Wheels


class Bot:

    def __init__(self):
        self.wheels = Wheels()
        pass

    def move(self, left, right, time):
        self.wheels.move(left, right, time)

    def start_camera(self):
        start_camera()
