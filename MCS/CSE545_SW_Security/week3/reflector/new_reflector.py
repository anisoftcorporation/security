#!/usr/bin/env python3

from scapy.all import *

victimIp = ''
victimMac = ''
reflectorIp = ''
reflectorMac = ''
interface = ''


def call_back(packet):
    if ARP in packet:
        print(packet.show())


sniff(iface='wlan0', prn=call_back, store=0, count=0)


