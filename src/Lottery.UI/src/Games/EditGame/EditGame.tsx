import { useMutation, useQuery } from "@tanstack/react-query";
import { useNavigate, useParams } from "react-router-dom";
import gameService from "../gameService";
import { useForm, zodResolver } from "@mantine/form";
import {
  EditGamePrizeRequest,
  EditGameRequestBody,
  EditGameResponse,
  editGameRequestBodySchema,
} from "./editGame.schema";
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
  Skeleton,
  Text,
  TextInput,
  Title,
} from "@mantine/core";
import { allStatesWithLabel } from "../../common/schemas";
import { DateTimePicker } from "@mantine/dates";
import React, { useEffect } from "react";
import { IconTrash } from "@tabler/icons-react";

const placeholders: EditGameRequestBody = {
  name: "placeholder",
  closeTime: new Date(),
  drawTime: new Date(),
  maxSelections: 1,
  prizes: [
    { position: 1, numberMatchCount: 5 },
    { position: 2, numberMatchCount: 4 },
    { position: 3, numberMatchCount: 3 },
  ],
  selectionsRequiredForEntry: 5,
  startTime: new Date(),
  state: "enabled",
};

// TODO: Alot of duplication between EditGame and CreateGame forms
// Could do with using some shared components to reduce this duplication

interface EditGameProps {}

interface Params extends Record<string, string | undefined> {
  id: string;
}

function EditGame({}: EditGameProps) {
  const { id } = useParams<Params>();
  const navigate = useNavigate();

  const query = useQuery({
    queryKey: ["game", id],
    queryFn: async () => await gameService.getGame({ route: { id: id! } }),
    refetchInterval: 0,
  });

  const mutation = useMutation<EditGameResponse, unknown, EditGameRequestBody>({
    mutationFn: async (request) =>
      gameService.editGame({ body: request, route: { id: id! } }),
    onSuccess: async (response) => navigate(`/games/${response.id}`),
  });

  const form = useForm<EditGameRequestBody>({
    validate: zodResolver(editGameRequestBodySchema),
    validateInputOnChange: true,
    initialValues: {
      closeTime: query.data?.closeTime ?? placeholders.closeTime,
      drawTime: query.data?.drawTime ?? placeholders.drawTime,
      name: query.data?.name ?? placeholders.name,
      maxSelections:
        query.data?.selections.length ?? placeholders.maxSelections,
      prizes: query.data?.prizes ?? placeholders.prizes,
      selectionsRequiredForEntry:
        query.data?.selectionsRequiredForEntry ??
        placeholders.selectionsRequiredForEntry,
      startTime: query.data?.startTime ?? placeholders.startTime,
      state: query.data?.state ?? placeholders.state,
    },
  });

  useEffect(() => {
    if (!query.isLoading && query.data) {
      form.setValues({
        ...query.data,
        maxSelections: query.data.selections.length,
      });
    }
  }, [query.isLoading, query.data]);

  const colProps: GridColProps = {
    span: { xs: 12, sm: 6, md: 6, lg: 6 },
    style: { textAlign: "left" },
  };

  const dateColProps: GridColProps = {
    span: { xs: 12, sm: 4, md: 4, lg: 4 },
    style: { textAlign: "left" },
  };

  return (
    <Container p={0}>
      <Skeleton visible={query.isLoading}>
        <Title>Edit Game</Title>
      </Skeleton>

      <Paper shadow="xl" p={24} radius={10}>
        <form
          onSubmit={form.onSubmit(async (data) => mutation.mutateAsync(data))}
        >
          <Grid justify="center" gutter={"xl"}>
            <Grid.Col key={"name-col"} {...colProps}>
              <Skeleton visible={query.isLoading}>
                <TextInput
                  label="Name"
                  {...form.getInputProps("name")}
                  withAsterisk
                />
              </Skeleton>
            </Grid.Col>
            <Grid.Col key={"state-col"} {...colProps}>
              <Skeleton visible={query.isLoading}>
                <Select
                  label="State"
                  {...form.getInputProps("state")}
                  data={Object.values(allStatesWithLabel)}
                  withAsterisk
                  allowDeselect={false}
                />
              </Skeleton>
            </Grid.Col>
            <Grid.Col key={"startTime-col"} {...dateColProps}>
              <Skeleton visible={query.isLoading}>
                <DateTimePicker
                  label="Start Time"
                  {...form.getInputProps("startTime")}
                  withAsterisk
                />
              </Skeleton>
            </Grid.Col>
            <Grid.Col key={"closeTime-col"} {...dateColProps}>
              <Skeleton visible={query.isLoading}>
                <DateTimePicker
                  label="Close Time"
                  {...form.getInputProps("closeTime")}
                  withAsterisk
                />
              </Skeleton>
            </Grid.Col>
            <Grid.Col key={"drawTime-col"} {...dateColProps}>
              <Skeleton visible={query.isLoading}>
                <DateTimePicker
                  label="Draw Time"
                  {...form.getInputProps("drawTime")}
                  withAsterisk
                />
              </Skeleton>
            </Grid.Col>
            <Grid.Col key={"maxSelections-col"} {...colProps}>
              <Skeleton visible={query.isLoading}>
                <NumberInput
                  label="Selections in game"
                  {...form.getInputProps("maxSelections")}
                  withAsterisk
                />
              </Skeleton>
            </Grid.Col>
            <Grid.Col key={"selectionsRequiredForEntry-col"} {...colProps}>
              <Skeleton visible={query.isLoading}>
                <NumberInput
                  label="Selections per entry"
                  {...form.getInputProps("selectionsRequiredForEntry")}
                  withAsterisk
                />
              </Skeleton>
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

                {/* Updating prizes not yet supported on the server side. Changes will be ignored */}
                {form.values.prizes
                  .sort((a, b) => a.position - b.position)
                  .map((_, index) => (
                    <React.Fragment key={`prize-${index}`}>
                      <Grid.Col
                        key={`prize-${index}-position`}
                        mt={"xs"}
                        span={6}
                      >
                        <Skeleton visible={query.isLoading}>
                          <NumberInput
                            {...form.getInputProps(`prizes.${index}.position`)}
                            leftSection={
                              <ActionIcon
                                variant="transparent"
                                onClick={() =>
                                  form.removeListItem("prizes", index)
                                }
                                disabled={form.values.prizes.length <= 1}
                              >
                                <IconTrash />
                              </ActionIcon>
                            }
                          />
                        </Skeleton>
                      </Grid.Col>
                      <Grid.Col
                        key={`prize-${index}-numberMatchCount`}
                        mt={"xs"}
                        span={6}
                      >
                        <Skeleton visible={query.isLoading}>
                          <NumberInput
                            {...form.getInputProps(
                              `prizes.${index}.numberMatchCount`
                            )}
                          />
                        </Skeleton>
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
                    } as EditGamePrizeRequest)
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

export default EditGame;
