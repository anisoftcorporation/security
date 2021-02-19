#pip install --trusted-host pypi.org --trusted-host files.pythonhosted.org subprocess.run --user
#pip install --trusted-host pypi.org --trusted-host files.pythonhosted.org pandas --user
import subprocess
import re
import pandas as pd
import time
def clear(): 
    # check and make call for specific operating system 
    _ = subprocess.call('clear') 
def MITMCheck():
 sr = pd.Series(dtype=object);
 result = subprocess.run(['arp', '-a'], stdout=subprocess.PIPE)
 data = result.stdout
 for line in data.splitlines():
   
    mo = re.search(r'\((.*)\)', line.decode('utf-8'))
    if mo:
     ip = mo.group(1)
    macs = re.findall('([a-z]*[0-9]*:[a-z]*[0-9]*)', line.decode('utf-8'))
    mac = ''.join(macs)
    sr.at[ip]=mac
 print(sr)
 check = any(sr.duplicated(keep=False))
 print(check)
clear()
while True:
    MITMCheck()
    time.sleep(2)