import { useMutation } from "@tanstack/react-query";
import {
  CreateGameRequest,
  CreateGameResponse,
  createGameRequestSchema,
} from "./createGame.schema";
import gameService from "../gameService";
import { useNavigate } from "react-router-dom";

import { DateInput } from "@mantine/dates";
import CreateGamePrizes from "./CreateGamePrizes";
import {
  Button,
  Grid,
  NumberInput,
  Select,
  TextInput,
  Title,
} from "@mantine/core";

import { FormProvider, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { AllStates, State, States } from "../../common/schemas";

interface CreateGameProps {}

function getDate(now: Date, daysToAdd: number) {
  const date = new Date(now);
  date.setDate(now.getDate() + daysToAdd);
  date.setHours(0);
  date.setMinutes(0);
  date.setSeconds(0);
  date.setUTCMilliseconds(0);
  return date;
}

// TODO: There's a bit of a mashup of packages being used here. Need to sort them out.
/*
  useForm:
    - react-hook-form
    - @mantine/form
  zodResolver:
    - mantine-form-zod-resolver
    - @hookform/resolvers/zod

  Decision: Use hook form or mantine?
*/

function CreateGame({}: CreateGameProps) {
  const navigate = useNavigate();

  const mutation = useMutation<CreateGameResponse, unknown, CreateGameRequest>({
    mutationFn: async (request) => gameService.createGame(request),
    onSuccess: async (response) => navigate(`/games/${response.id}`),
  });

  const form = useForm<CreateGameRequest>({
    resolver: zodResolver(createGameRequestSchema),
  });

  const now = new Date();
  const defaultStartTime = getDate(now, 1);
  const defaultDrawTime = getDate(now, 7);

  return (
    <FormProvider {...form}>
      <form
        onSubmit={form.handleSubmit(async (data) => mutation.mutateAsync(data))}
      >
        <Title>Create Game</Title>
        <Grid justify="center" gutter={"xl"}>
          <Grid.Col key={"name"} span={{ xs: 12, sm: 6, md: 6, lg: 6 }}>
            <TextInput
              label="Name"
              {...form.register("name")}
              defaultValue={`Lottery Game - ${new Date().toDateString()}`}
              withAsterisk
            />
          </Grid.Col>
          <Grid.Col key={"name"} span={{ xs: 12, sm: 6, md: 6, lg: 6 }}>
            <Select
              label="State"
              {...form.register("state")}
              onChange={(v) => form.setValue("state", v as State)}
              data={AllStates.map((s) => ({ value: s, label: s }))}
              defaultValue={States.Enabled}
              withAsterisk
              allowDeselect={false}
            />
          </Grid.Col>
          <Grid.Col key={"name"} span={{ xs: 12, sm: 6, md: 6, lg: 6 }}>
            <DateInput
              label="Start Time"
              valueFormat="DD/MM/YYYY HH:mm:ss"
              defaultValue={defaultStartTime}
              {...form.register("startTime")}
              onChange={(v) => form.setValue("startTime", v as Date)}
            />
          </Grid.Col>
          <Grid.Col key={"name"} span={{ xs: 12, sm: 6, md: 6, lg: 6 }}>
            <DateInput
              label="Draw Time"
              valueFormat="DD/MM/YYYY HH:mm:ss"
              defaultValue={defaultDrawTime}
              {...form.register("drawTime")}
              onChange={(v) => form.setValue("drawTime", v as Date)}
            />
          </Grid.Col>
          <Grid.Col key={"name"} span={{ xs: 12, sm: 6, md: 6, lg: 6 }}>
            <NumberInput
              label="Selections in game"
              {...form.register("maxSelections")}
              onChange={(v) => form.setValue("maxSelections", Number(v))}
              defaultValue={50}
              min={3}
              max={100}
              withAsterisk
            />
          </Grid.Col>
          <Grid.Col key={"name"} span={{ xs: 12, sm: 6, md: 6, lg: 6 }}>
            <NumberInput
              label="Selections per entry"
              {...form.register("selectionsRequiredForEntry")}
              onChange={(v) =>
                form.setValue("selectionsRequiredForEntry", Number(v))
              }
              defaultValue={5}
              min={3}
              max={100}
              withAsterisk
            />
          </Grid.Col>
          <Grid.Col span={12}>
            <CreateGamePrizes onChange={(v) => form.setValue("prizes", v)} />
          </Grid.Col>

          <Grid.Col
            span={{ xs: 3.5, sm: 2.5, md: 2.5, lg: 2.5, xl: 2.5, mt: 10 }}
          >
            <Button
              type="submit"
              loading={form.formState.isSubmitting}
              fullWidth
            >
              {form.formState.isSubmitting ? "Submitting" : "Submit"}
            </Button>
          </Grid.Col>
          <Grid.Col
            span={{ xs: 3.5, sm: 2.5, md: 2.5, lg: 2.5, xl: 2.5, mt: 10 }}
          >
            <Button
              fullWidth
              onClick={() => {
                console.log(form.getValues());
              }}
            >
              Log Form Values
            </Button>
          </Grid.Col>
        </Grid>
      </form>
    </FormProvider>
  );
}

export default CreateGame;