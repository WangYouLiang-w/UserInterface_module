import socket
import time




client = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)

addr = ('127.0.0.1',7810)


LableNum  = [1,3,4,5,2,6,8,9,7,10]


while True:
    for label in LableNum:
        time.sleep(0.5)
        print(label)
        command = bytes(str(label),'Utf8')
        client.sendto(command, addr)

        
