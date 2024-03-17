package org.tratnjek.trackmanagementsystem;

import io.quarkus.grpc.GrpcService;
import io.smallrye.common.annotation.Blocking;
import io.smallrye.mutiny.Uni;
import jakarta.inject.Inject;
import jakarta.transaction.Transactional;
import jakarta.validation.ConstraintViolation;
import jakarta.validation.Validator;

import java.util.List;
import java.util.Set;
import java.util.logging.Logger;
import java.util.stream.Collectors;

@GrpcService
public class TaskService implements TaskGrpcService {

    @Inject
    Validator validator;

    private final Logger logger = Logger.getLogger(TaskService.class.getName());

    @Blocking
    @Transactional
    @Override
    public Uni<AddTaskResponse> addTask(AddTaskRequest request) {
        logger.info("Adding task");

        logger.info("Creating task object from request");
        Task task = new Task();
        task.name = request.getName();
        task.description = request.getDescription();
        task.status = request.getStatus();
        task.priority = request.getPriority();
        task.category = request.getCategory();

        Set<ConstraintViolation<Task>> violations = validator.validate(task);

        logger.info("Validating task object");
        if(!violations.isEmpty())
        {
            var message = violations.stream()
                    .map(v -> v.getPropertyPath() + " " + v.getMessage())
                    .collect(Collectors.joining(", "));

            logger.info("Validation failed. Returning response.");
            return Uni.createFrom().item(() -> AddTaskResponse.newBuilder()
                    .setSuccess(false)
                    .setMessage(message)
                    .build());
        }

        logger.info("Persisting task object");
        task.persist();

        logger.info("Task object persisted successfully. Returning response.");
        return Uni.createFrom().item(() -> AddTaskResponse.newBuilder()
                .setSuccess(true)
                .build());

    }


    @Blocking
    @Transactional
    @Override
    public Uni<GetTaskResponse> getTask(GetTaskRequest request) {

        logger.info("Getting task");
        logger.info(request.toString());

        Task task = Task.findById(Long.parseLong(request.getId()));

        if (task == null) {
            logger.info("Task not found. Returning response.");
            return Uni.createFrom().item(() -> GetTaskResponse.newBuilder()
                    .setSuccess(false)
                    .setMessage("Task not found")
                    .build());
        }

        logger.info("Task found. Returning response.");
        return Uni.createFrom().item(() -> GetTaskResponse.newBuilder()
                .setName(task.name)
                .setDescription(task.description)
                .setStatus(task.status)
                .setPriority(task.priority)
                .setCategory(task.category)
                .build());

    }

    @Blocking
    @Transactional
    @Override
    public Uni<UpdateTaskResponse> updateTask(UpdateTaskRequest request) {

        logger.info("Updating task");
        logger.info(request.toString());

        Task task = Task.findById(Long.parseLong(request.getId()));

        if (task == null) {
            return Uni.createFrom().item(() -> UpdateTaskResponse.newBuilder()
                    .setSuccess(false)
                    .setMessage("Task not found")
                    .build());
        }

        task.name = request.getName();
        task.description = request.getDescription();
        task.status = request.getStatus();
        task.priority = request.getPriority();
        task.category = request.getCategory();

        Set<ConstraintViolation<Task>> violations = validator.validate(task);

        logger.info("Validating task object");
        if(!violations.isEmpty())
        {
            var message = violations.stream()
                    .map(v -> v.getPropertyPath() + " " + v.getMessage())
                    .collect(Collectors.joining(", "));

            logger.info("Validation failed. Returning response.");
            return Uni.createFrom().item(() -> UpdateTaskResponse.newBuilder()
                    .setSuccess(false)
                    .setMessage(message)
                    .build());
        }

        task.persist();

        return Uni.createFrom().item(() -> UpdateTaskResponse.newBuilder()
                .setSuccess(true)
                .build());

    }

    @Blocking
    @Transactional
    @Override
    public Uni<DeleteTaskResponse> deleteTask(DeleteTaskRequest request) {

        logger.info("Deleting task");
        logger.info(request.toString());

        Task task = Task.findById(Long.parseLong(request.getId()));

        if (task == null) {
            return Uni.createFrom().item(() -> DeleteTaskResponse.newBuilder()
                    .setSuccess(false)
                    .setMessage("Task not found")
                    .build());
        }

        task.delete();

        return Uni.createFrom().item(() -> DeleteTaskResponse.newBuilder()
                .setSuccess(true)
                .build());
    }

    @Blocking
    @Transactional
    @Override
    public Uni<ListTaskResponse> listTasks(ListTaskRequest request) {

        logger.info("Listing tasks");

        List<Task> tasks;

        if(request.hasStatus() && request.hasPriority()){
            tasks = Task.list("status = ?1 and priority = ?2 and category = ?3", request.getStatus(), request.getPriority(), request.getCategory());
        }
        else if(request.hasStatus()){
            tasks = Task.list("status = ?1 and category = ?2", request.getStatus(), request.getCategory());
        }
        else if(request.hasPriority()){
            tasks = Task.list("priority = ?1 and category = ?2", request.getPriority(), request.getCategory());
        }
        else{
            tasks = Task.list("category = ?1", request.getCategory());
        }

        // Convert List<Task> to List<TaskItem>
        List<TaskItem> taskItems = tasks.stream().map(task -> {
            TaskItem.Builder taskItemBuilder = TaskItem.newBuilder()
                    .setId(task.id.toString())
                    .setName(task.name)
                    .setDescription(task.description)
                    .setCategory(task.category)
                    .setStatus(task.status)
                    .setPriority(task.priority);
            return taskItemBuilder.build();
        }).collect(Collectors.toList());

        // Create ListTaskResponse
        ListTaskResponse response = ListTaskResponse.newBuilder()
                .addAllTasks(taskItems)
                .build();

        // return tasks
        return Uni.createFrom().item(response);
    }
}
