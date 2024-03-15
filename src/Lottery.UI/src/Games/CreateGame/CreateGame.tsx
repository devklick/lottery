import { useMutation } from "@tanstack/react-query";
import {
  CreateGameRequest,
  CreateGameResponse,
  createGameRequestSchema,
} from "./createGame.schema";
import gameService from "../gameService";
import { useNavigate } from "react-router-dom";
import {
  Controller,
  FormProvider,
  useController,
  useForm,
} from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { InputError, Stack, TextInput, Title } from "@mantine/core";
import { DateInput, DatePicker, DateTimePicker } from "@mantine/dates";
import { useEffect } from "react";
import Form from "../../components/Form";

interface CreateGameProps {}

function CreateGame({}: CreateGameProps) {
  return (
    <Form<CreateGameRequest>
      schema={createGameRequestSchema}
      onSubmit={gameService.createGame}
      fields={{
        drawTime: {
          control: "date-input",
          type: "date",
          label: "Draw Time",
          withAsterisk: true,
          allowDeselect: false,
        },
        maxSelections: {
          control: "number-input",
          label: "Maximum Selections",
          withAsterisk: true,
          min: 3,
          max: 100,
        },
        name: {
          control: "text-input",
          label: "Name",
          withAsterisk: true,
        },
        prizes: {
          control: "text-area",
          label: "Prizes (To do)",
        },
        selectionsRequiredForEntry: {
          control: "number-input",
          label: "Selections Required For Entry",
          withAsterisk: true,
          min: 3,
          max: 100,
          // TODO: Validate against maxSelections value
        },
        startTime: {
          control: "date-input",
          type: "date",
          label: "Start Time",
          withAsterisk: true,
          allowDeselect: false,
        },
        state: {
          control: "switch",
          label: "Enabled",
          withAsterisk: true,
        },
      }}
    ></Form>
  );
  // const navigate = useNavigate();

  // const form = useForm<CreateGameRequest>({
  //   resolver: zodResolver(createGameRequestSchema),
  // });

  // function handleGameCreated(response: CreateGameResponse) {
  //   navigate(`games/${response.id}`);
  // }

  // const mutation = useMutation<CreateGameResponse, unknown, CreateGameRequest>({
  //   mutationFn: async (request) => await gameService.createGame(request),
  //   onSuccess: handleGameCreated,
  // });

  // function FieldError({ name }: { name: keyof CreateGameRequest }) {
  //   // const message = errors[name]?.message;
  //   // return message && <InputError>{message}</InputError>;
  // }

  // useEffect(() => {
  //   console.log(form.getValues());
  // }, [form]);

  // const {
  //   field: startTimeField,
  //   fieldState: { error: startTimeError },
  // } = useController<CreateGameRequest>({
  //   name: "startTime",
  //   control: form.control,
  // });

  // const startTimeInputError = startTimeError && (
  //   <InputError>{startTimeError.message}</InputError>
  // );

  // return (
  //   <FormProvider {...form}>
  //     <form
  //       onSubmit={form.handleSubmit(async (d) => await mutation.mutateAsync(d))}
  //     >
  //       <Stack gap={24}>
  //         <Title>Create Game</Title>

  //         <TextInput placeholder="Name" {...form.register("name")} />
  //         {/* <FieldError name="name" /> */}

  //         <DateInput
  //           id={"startTime"}
  //           label={"Start Time"}
  //           {...startTimeField}
  //           error={startTimeInputError}
  //           value={undefined}
  //         />
  //       </Stack>
  //     </form>
  //   </FormProvider>
  // );
  // return "hello from create games";
}

export default CreateGame;
