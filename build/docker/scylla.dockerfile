FROM scylladb/scylla:latest
COPY ./scylla-init.sh /
RUN chmod +x /scylla-init.sh
ENTRYPOINT ["/scylla-init.sh"]