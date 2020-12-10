#!/bin/sh

echo "*********************************************************************"
echo "*********************************************************************"
echo "KEYSPACE: $CASSANDRA_KEYSPACE"
echo "*********************************************************************"
echo "*********************************************************************"

/docker-entrypoint.py "$@" &

if [[ ! -z "$CASSANDRA_KEYSPACE" ]]; then
    # Create default keyspace for single node cluster
    CQL="CREATE KEYSPACE $CASSANDRA_KEYSPACE WITH REPLICATION = {'class': 'SimpleStrategy', 'replication_factor': 1};"
    until echo $CQL | cqlsh; do
        echo "*********************************************************************"
        echo "*********************************************************************"
        echo "cqlsh: Cassandra is unavailable - retry later"
        echo "*********************************************************************"
        echo "*********************************************************************"

        sleep 10
    done &
fi

wait
