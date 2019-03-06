aws s3api list-objects --bucket storage-servelessbenchmark.cjrp.me --output text --query Contents[].Key > object-list.txt
