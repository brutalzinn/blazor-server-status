#to easily my tests 
#https://stackoverflow.com/questions/46029362/python-why-while-true-in-a-thread-is-blocking-another-thread

import threading
import docker
client = docker.from_env()
def restart(container_name: str):
	while True:
		print("start container {}".format(container_name))
		container = client.containers.get(container_name)
		container.stop()
		container.wait()
		container.start()
		print("container {}".format(container_name))
	os.sleep(15)

t1 = threading.Thread(target=restart, args=["container_teste"])
t1.start()
