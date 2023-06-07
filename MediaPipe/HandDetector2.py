import cv2
import mediapipe as mp
import time 
import socket
import math
import numpy as np
import os
import random
from scipy.spatial import distance
from threading import Thread

# defining a helper class for implementing multi-threaded processing 
class WebcamStream :
    def __init__(self, stream_id=0, width=1920, height=1080):
        self.stream_id = stream_id   # default is 0 for primary camera 
        
        # opening video capture stream 
        self.vcap = cv2.VideoCapture(self.stream_id,cv2.CAP_DSHOW)
        self.vcap.set(3,width)
        self.vcap.set(4,height)
        if self.vcap.isOpened() is False :
            print("[Exiting]: Error accessing webcam stream.")
            exit(0)
        fps_input_stream = int(self.vcap.get(5))
        print("FPS of webcam hardware/input stream: {}".format(fps_input_stream))
            
        # reading a single frame from vcap stream for initializing 
        self.grabbed , self.frame = self.vcap.read()
        if self.grabbed is False :
            print('[Exiting] No more frames to read')
            exit(0)

        # self.stopped is set to False when frames are being read from self.vcap stream 
        self.stopped = True 

        # reference to the thread for reading next available frame from input stream 
        self.t = Thread(target=self.update, args=())
        self.t.daemon = True # daemon threads keep running in the background while the program is executing 
        
    # method for starting the thread for grabbing next available frame in input stream 
    def start(self):
        self.stopped = False
        self.t.start() 

    # method for reading next frame 
    def update(self):
        while True :
            if self.stopped is True :
                break
            self.grabbed , self.frame = self.vcap.read()
            if self.grabbed is False :
                print('[Exiting] No more frames to read')
                self.stopped = True
                break 
        self.vcap.release()

    # method for returning latest read frame 
    def read(self):
        return self.frame

    # method called to stop reading frames 
    def stop(self):
        self.stopped = True 

HEIGHT, WIDTH = 720, 1280
"""
params:
"""
def detectHands(img, hands):
    imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
    results = hands.process(imgRGB)
    handL = []
    handR = []
    if results.multi_hand_landmarks:
        for handType, handLms in zip(results.multi_handedness, results.multi_hand_landmarks):
            landmarks = []
            for lm in handLms.landmark:
                point = (WIDTH - int(lm.x * WIDTH),HEIGHT - int(lm.y * HEIGHT), HEIGHT - int(lm.z * HEIGHT))
                landmarks.extend(point)

            if handType.classification[0].label == "Right":
                handL = landmarks
            else:
                handR = landmarks
    return handR, handL

def GetDepthMono(point1, point2):
    normalizedFocaleX = 1.40625
    fx = min(WIDTH, HEIGHT) * normalizedFocaleX
    dX = 15.2
    dx = distance.euclidean(point1, point2)
    dZ = (fx * (dX / dx))*10
    return (int(dZ))

def GetDepthPoints(list, landmark1, landmark2):
    point1 = (list[landmark1*3+0],list[landmark1*3+1],list[landmark1*3+2])
    point2 = (list[landmark2*3+0],list[landmark2*3+1],list[landmark2*3+2])

    print(point1, point2)
    depth = GetDepthMono(point1,point2)

    for i, l in enumerate(list):
        if(i%3 == 2):
            list[i] = int(depth - HEIGHT + l)
    return list


def GetHandWithDepth(list1, list2):
  if(len(list1) != 0 and len(list2) != 0):
    for i in range(0,int(len(list1)/3)):
        z = Depth(list1[3*i],list2[3*i])
        if(z != -1):
            list1[3*i + 2] = z
  return list1

def Depth(pointL, pointR):
        dissimilarity = abs(pointL - pointR)
        if(dissimilarity != 0):
            return int(100000 / dissimilarity)
        else:
            return -1


start_time = time.time()    


webcam1 = WebcamStream(0,WIDTH,HEIGHT) 
webcam1.start()
webcam2 = WebcamStream(0,WIDTH,HEIGHT) 
webcam2.start()

print(time.time() - start_time)
start_time = time.time()

# Initialize the mediapipe hand detector
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(
            static_image_mode=False,
            max_num_hands=2,
            min_detection_confidence=0.8)

# Initialize the Socket
sock = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
port = ("127.0.0.1",3333)
num_frames = 0

depth_track = []

# Video Capture Loop
while True:
    img1 = webcam1.read()
    img2 = webcam2.read()
    rightHand1, leftHand1 = detectHands(img1, hands) 

    if(len(rightHand1) != 0):
        rightHand1 = GetDepthPoints(rightHand1,0,20)
    if(len(leftHand1) != 0):
        leftHand1 = GetDepthPoints(leftHand1,0,20)

    #rightHand2, leftHand2 = detectHands(img2, hands)
    #rightHand = GetHandWithDepth(rightHand1,rightHand2)
    #leftHand = GetHandWithDepth(leftHand1,leftHand2)
    strToSend = str.encode(str(rightHand1)) + str.encode("Left:") + str.encode(str(leftHand1))
    """
    if(len(rightHand1) != 0):
        point1 = (rightHand1[0],rightHand1[1],rightHand1[1])
        point2 = (rightHand1[15],rightHand1[16],rightHand1[17])
        z = GetDepthMono(point1,point2)
        depth_track.append(z)
    else:
        depth_track.append(-10000)
    """

    sock.sendto(strToSend,port)

    num_frames+=1
    if(num_frames%100 == 0):
        print(num_frames/(time.time()-start_time))


print((time.time()-start_time))

success = False
while not success:
    rnd = random.randint(0,1000000000)
    absPathMono = "C:\\Users\\camar\\OneDrive\\Escritorio\\TFG\\MediaPipe\\Test\\Mono\\Close"
    absPathStereo = "C:\\Users\\camar\\OneDrive\\Escritorio\\TFG\\MediaPipe\\Test\\Stereo\\Rotation"
    path = absPathMono + "\\DepthTrack-" + str(rnd) + ".csv"
    if(not os.path.isfile(path)):
        print(path)
        success = True
        np.savetxt(path, 
                depth_track,
                delimiter =", ", 
                fmt ='% s')
