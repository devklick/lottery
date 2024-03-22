import { useMutation } from "@tanstack/react-query";
import {
  CreateGamePrizeRequest,
  CreateGameRequest,
  CreateGameResponse,
  createGameRequestSchema,
} from "./createGame.schema";
import gameService from "../gameService";
import { useNavigate } from "react-router-dom";

import { DateTimePicker } from "@mantine/dates";
import {
  ActionIcon,
  Button,
  Container,
  Grid,
  GridColProps,
  Group,
  NumberInput,
  Paper,
  Select,
  Text,
  TextInput,
  Title,
} from "@mantine/core";

import { useForm } from "@mantine/form";
import { zodResolver } from "mantine-form-zod-resolver";
import { allStatesWithLabel } from "../../common/schemas";
import { IconTrash } from "@tabler/icons-react";
import React from "react";

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
  const defaultDrawTime = getDate(now, 8);

  const initialValues: CreateGameRequest = {
    name: `Lottery Game - ${now.toDateString()}`,
    state: "enabled",
    startTime: defaultStartTime,
    drawTime: defaultDrawTime,
    maxSelections: 50,
    selectionsRequiredForEntry: 5,
    prizes: [{ position: 1, numberMatchCount: 5 }],
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

  const colProps: GridColProps = {
    span: { xs: 12, sm: 6, md: 6, lg: 6 },
    style: { textAlign: "left" },
  };

  return (
    <Container p={0}>
      <Title>Create Game</Title>
      <Paper shadow="xl" p={24} radius={10}>
        <form
          onSubmit={form.onSubmit(async (data) => mutation.mutateAsync(data))}
        >
          <Grid justify="center" gutter={"xl"}>
            <Grid.Col key={"name-col"} {...colProps}>
              <TextInput
                label="Name"
                {...form.getInputProps("name")}
                withAsterisk
              />
            </Grid.Col>
            <Grid.Col key={"state-col"} {...colProps}>
              <Select
                label="State"
                {...form.getInputProps("state")}
                data={Object.values(allStatesWithLabel)}
                withAsterisk
                allowDeselect={false}
              />
            </Grid.Col>
            <Grid.Col key={"startTime-col"} {...colProps}>
              <DateTimePicker
                label="Start Time"
                {...form.getInputProps("startTime")}
                withAsterisk
              />
            </Grid.Col>
            <Grid.Col key={"drawTime-col"} {...colProps}>
              <DateTimePicker
                label="Draw Time"
                {...form.getInputProps("drawTime")}
                withAsterisk
              />
            </Grid.Col>
            <Grid.Col key={"maxSelections-col"} {...colProps}>
              <NumberInput
                label="Selections in game"
                {...form.getInputProps("maxSelections")}
                withAsterisk
              />
            </Grid.Col>
            <Grid.Col key={"selectionsRequiredForEntry-col"} {...colProps}>
              <NumberInput
                label="Selections per entry"
                {...form.getInputProps("selectionsRequiredForEntry")}
                withAsterisk
              />
            </Grid.Col>
            <Grid.Col key={"prizes-col"} span={12}>
              <Title key={"prizes-title"} size={"h2"}>
                Prizes
              </Title>
              <Grid key={"prizes-grid"} maw={500} mx="auto">
                <Grid.Col
                  key={"prize-position-header"}
                  mt={"xs"}
                  {...colProps}
                  span={6}
                >
                  <Text fw={500} size="sm">
                    Position
                  </Text>
                </Grid.Col>
                <Grid.Col
                  key={"prize-numberMatchCount-header"}
                  mt={"xs"}
                  {...colProps}
                  span={6}
                >
                  <Text key={"some key"} fw={500} size="sm">
                    Matching Numbers
                  </Text>
                </Grid.Col>

                {form.values.prizes.map((_, index) => (
                  <React.Fragment key={`prize-${index}`}>
                    <Grid.Col
                      key={`prize-${index}-position`}
                      mt={"xs"}
                      span={6}
                    >
                      <NumberInput
                        {...form.getInputProps(`prizes.${index}.position`)}
                        leftSection={
                          <ActionIcon
                            variant="transparent"
                            onClick={() => form.removeListItem("prizes", index)}
                            disabled={form.values.prizes.length <= 1}
                          >
                            <IconTrash />
                          </ActionIcon>
                        }
                      />
                    </Grid.Col>
                    <Grid.Col
                      key={`prize-${index}-numberMatchCount`}
                      mt={"xs"}
                      span={6}
                    >
                      <NumberInput
                        {...form.getInputProps(
                          `prizes.${index}.numberMatchCount`
                        )}
                      />
                    </Grid.Col>
                  </React.Fragment>
                ))}
              </Grid>
              <Group justify="center" mt={"md"}>
                <Button
                  onClick={() =>
                    form.insertListItem("prizes", {
                      numberMatchCount: 1,
                      position: 1,
                    } as CreateGamePrizeRequest)
                  }
                >
                  Add new prize
                </Button>
              </Group>
            </Grid.Col>

            <Grid.Col
              key={"submit-col"}
              span={{ xs: 3.5, sm: 2.5, md: 2.5, lg: 2.5, xl: 2.5, mt: 10 }}
            >
              <Group mt={50}>
                <Button type="submit" loading={mutation.isPending} fullWidth>
                  {mutation.isPending ? "Submitting" : "Submit"}
                </Button>
              </Group>
            </Grid.Col>
          </Grid>
        </form>
      </Paper>
    </Container>
  );
}

export default CreateGame;
