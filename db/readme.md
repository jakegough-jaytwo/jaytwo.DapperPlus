# Database Containers

In the `docker-compose.yml` there are containers in two profiles:
  * `localdev`: starts containers with ports exposed so you can develop locally against them
  * `testernet`: starts containers with __no ports exposed__ so Jenkins can attach the unit test to the docker network to run tests without worrying about port conflicts

```
// to start sample databases for unit tests
make localdev

// to view local dev database logs
make localdev-logs

// to stop the local dev databases
make localdev-clean
```

```
// inside the jenkinsfile it will need
make testernet-up
make testernet-run
make testernet-clean
```
