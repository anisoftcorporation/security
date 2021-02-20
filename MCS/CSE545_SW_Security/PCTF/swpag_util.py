import swpag_client
t = swpag_client.Team('http://34.211.129.130','PCRz3IhqjCJ1SlDqRVuH')
services = t.get_service_list()
for service in services:
    print(service['service_name'])
    targets = t.get_targets(service['service_id'])
    for target in targets:
        print(target['hostname']+':'+target['flag_id'])

