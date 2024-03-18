import { useMutation } from "@tanstack/react-query";
import {
  CreateGameRequest,
  CreateGameResponse,
  createGameRequestSchema,
} from "./createGame.schema";
import gameService from "../gameService";
import { useNavigate } from "react-router-dom";

import { DateTimePicker } from "@mantine/dates";
import CreateGamePrizes from "./CreateGamePrizes";
import {
  Button,
  Grid,
  NumberInput,
  Select,
  TextInput,
  Title,
} from "@mantine/core";

import { useForm } from "@mantine/form";
import { zodResolver } from "mantine-form-zod-resolver";
import { AllStates } from "../../common/schemas";

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

function CreateGame({}: CreateGameProps) {
  const now = new Date();
  const defaultStartTime = getDate(now, 1);
  const defaultDrawTime = getDate(now, 7);

  const initialValues: CreateGameRequest = {
    name: `Lottery Game - ${now.toDateString()}`,
    state: "Enabled",
    startTime: defaultStartTime,
    drawTime: defaultDrawTime,
    maxSelections: 50,
    selectionsRequiredForEntry: 5,
    prizes: [],
  };

  const navigate = useNavigate();

  const mutation = useMutation<CreateGameResponse, unknown, CreateGameRequest>({
    mutationFn: async (request) => gameService.createGame(request),
    onSuccess: async (response) => navigate(`/games/${response.id}`),
  });

  const form = useForm<CreateGameRequest>({
    validate: zodResolver(createGameRequestSchema),
    validateInputOnChange: true,
    initialValues: initialValues,
  });

  return (
    // <FormProvider {...form}>
    <form onSubmit={form.onSubmit(async (data) => mutation.mutateAsync(data))}>
      <Title>Create Game</Title>
      <Grid justify="center" gutter={"xl"}>
        <Grid.Col key={"name"} span={{ xs: 12, sm: 6, md: 6, lg: 6 }}>
          <TextInput
            label="Name"
            {...form.getInputProps("name")}
            withAsterisk
          />
        </Grid.Col>
        <Grid.Col key={"state"} span={{ xs: 12, sm: 6, md: 6, lg: 6 }}>
          <Select
            label="State"
            {...form.getInputProps("state")}
            data={AllStates.map((s) => ({ value: s, label: s }))}
            withAsterisk
            allowDeselect={false}
          />
        </Grid.Col>
        <Grid.Col key={"startTime"} span={{ xs: 12, sm: 6, md: 6, lg: 6 }}>
          <DateTimePicker
            label="Start Time"
            {...form.getInputProps("startTime")}
            withAsterisk
          />
        </Grid.Col>
        <Grid.Col key={"drawTime"} span={{ xs: 12, sm: 6, md: 6, lg: 6 }}>
          <DateTimePicker
            label="Draw Time"
            {...form.getInputProps("drawTime")}
            withAsterisk
          />
        </Grid.Col>
        <Grid.Col key={"maxSelections"} span={{ xs: 12, sm: 6, md: 6, lg: 6 }}>
          <NumberInput
            label="Selections in game"
            {...form.getInputProps("maxSelections")}
            withAsterisk
          />
        </Grid.Col>
        <Grid.Col
          key={"selectionsRequiredForEntry"}
          span={{ xs: 12, sm: 6, md: 6, lg: 6 }}
        >
          <NumberInput
            label="Selections per entry"
            {...form.getInputProps("selectionsRequiredForEntry")}
            withAsterisk
          />
        </Grid.Col>
        <Grid.Col span={12}>
          <CreateGamePrizes onChange={(v) => form.setFieldValue("prizes", v)} />
        </Grid.Col>

        <Grid.Col
          span={{ xs: 3.5, sm: 2.5, md: 2.5, lg: 2.5, xl: 2.5, mt: 10 }}
        >
          <Button type="submit" loading={mutation.isPending} fullWidth>
            {mutation.isPending ? "Submitting" : "Submit"}
          </Button>
        </Grid.Col>
        <Grid.Col
          span={{ xs: 3.5, sm: 2.5, md: 2.5, lg: 2.5, xl: 2.5, mt: 10 }}
        >
          <Button
            fullWidth
            onClick={() => {
              console.log(form.values);
            }}
          >
            Log Form Values
          </Button>
        </Grid.Col>
      </Grid>
    </form>
    // </FormProvider>
  );
}

export default CreateGame;
