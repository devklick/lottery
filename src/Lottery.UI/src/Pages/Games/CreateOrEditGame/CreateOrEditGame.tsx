import {
  ActionIcon,
  Grid,
  GridColProps,
  Group,
  NumberInput,
  Select,
  Stack,
  Text,
  TextInput,
  Title,
} from "@mantine/core";
import { DateTimePicker } from "@mantine/dates";
import { schemaResolver, useForm, UseFormReturnType } from "@mantine/form";
import { IconPlus, IconTrash } from "@tabler/icons-react";
import { useMutation } from "@tanstack/react-query";
import { Fragment } from "react/jsx-runtime";
import { useNavigate } from "react-router-dom";

import ActionButtons from "./ActionButtons";
import { allStatesWithLabel } from "../../../common/schemas";
import { toPascalCase } from "../../../common/utils/string.utils";
import Page from "../../../components/Page/Page";
import PageSection from "../../../components/PageSection/PageSection";
import {
  CreateGameRequest,
  createGameRequestSchema,
  CreateGameResponse,
} from "../CreateGame/createGame.schema";
import {
  EditGameRequestBody,
  editGameRequestBodySchema,
  EditGameResponse,
} from "../EditGame/editGame.schema";

type Shared<T, U> = {
  [K in Extract<keyof T, keyof U>]: T[K] & U[K];
};

type CreateOrEditFromValues = Shared<CreateGameRequest, EditGameRequestBody>;

interface CreateOrEditGameProps<
  Mode extends "edit" | "create",
  TResponse extends EditGameResponse | CreateGameResponse = Mode extends "edit"
    ? EditGameResponse
    : CreateGameResponse,
> {
  mode: Mode;
  initialValues: CreateOrEditFromValues;
  mutationFn(request: CreateOrEditFromValues): Promise<TResponse>;
  onCancel(): void;
  disabled?: boolean;
  submitDisabled(
    form: UseFormReturnType<CreateOrEditFromValues, CreateOrEditFromValues>,
  ): boolean;
}

export default function CreateOrEditGame<
  Mode extends "edit" | "create",
  TResponse extends EditGameResponse | CreateGameResponse = Mode extends "edit"
    ? EditGameResponse
    : CreateGameResponse,
>({
  mode,
  initialValues,
  mutationFn,
  onCancel,
  submitDisabled,
  disabled,
}: CreateOrEditGameProps<Mode, TResponse>) {
  const navigate = useNavigate();
  const mutation = useMutation({
    mutationFn,
    onSuccess: (response) => navigate(`/games/${response.id}`),
  });

  const schema =
    mode === "create" ? createGameRequestSchema : editGameRequestBodySchema;

  const form = useForm<CreateOrEditFromValues>({
    validate: schemaResolver(schema),
    transformValues: schema.parse,
    validateInputOnChange: true,
    validateInputOnBlur: true,
    initialValues,
  });

  form.getInputProps("");

  const colProps: GridColProps = {
    span: { xs: 12, sm: 6, md: 6, lg: 6 },
    style: { textAlign: "left" },
  };

  const dateColProps: GridColProps = {
    span: { xs: 12, sm: 4, md: 4, lg: 4 },
    style: { textAlign: "left" },
  };

  return (
    <Page
      title={{ value: `${toPascalCase(mode)} Game` }}
      children={
        <PageSection>
          <form
            id={`${mode}-game-form`}
            onSubmit={form.onSubmit(async (data) => mutation.mutateAsync(data))}
          >
            <Grid justify="center" gap={"xl"}>
              <Grid.Col key={"name-col"} {...colProps}>
                <TextInput
                  label="Name"
                  {...form.getInputProps("name")}
                  withAsterisk
                  disabled={disabled}
                />
              </Grid.Col>
              <Grid.Col key={"state-col"} {...colProps}>
                <Select
                  label="State"
                  {...form.getInputProps("state")}
                  data={Object.values(allStatesWithLabel)}
                  withAsterisk
                  allowDeselect={false}
                  disabled={disabled}
                />
              </Grid.Col>
              <Grid.Col key={"startTime-col"} {...dateColProps}>
                <DateTimePicker
                  label="Start Time"
                  {...form.getInputProps("startTime")}
                  withAsterisk
                  disabled={disabled}
                />
              </Grid.Col>
              <Grid.Col key={"closeTime-col"} {...dateColProps}>
                <DateTimePicker
                  label="Close Time"
                  {...form.getInputProps("closeTime")}
                  withAsterisk
                  disabled={disabled}
                />
              </Grid.Col>
              <Grid.Col key={"drawTime-col"} {...dateColProps}>
                <DateTimePicker
                  label="Draw Time"
                  {...form.getInputProps("drawTime")}
                  withAsterisk
                  disabled={disabled}
                />
              </Grid.Col>
              <Grid.Col key={"maxSelections-col"} {...colProps}>
                <NumberInput
                  label="Selections in game"
                  {...form.getInputProps("maxSelections")}
                  withAsterisk
                  disabled={disabled}
                />
              </Grid.Col>
              <Grid.Col key={"selectionsRequiredForEntry-col"} {...colProps}>
                <NumberInput
                  label="Selections per entry"
                  {...form.getInputProps("selectionsRequiredForEntry")}
                  withAsterisk
                  disabled={disabled}
                />
              </Grid.Col>
              <Grid.Col key={"prizes-col"} span={12}>
                <Title key={"prizes-title"} size={"h2"}>
                  Prizes
                </Title>
                <Grid key={"prizes-grid"} maw={500} mx="auto" mt={20}>
                  <Grid.Col
                    key={"prize-position-header"}
                    {...colProps}
                    span={5.5}
                  >
                    <Text fw={500} size="sm">
                      Position
                    </Text>
                  </Grid.Col>
                  <Grid.Col
                    key={"prize-numberMatchCount-header"}
                    {...colProps}
                    span={5.5}
                  >
                    <Text key={"some key"} fw={500} size="sm">
                      Matching Numbers
                    </Text>
                  </Grid.Col>
                  <Grid.Col
                    key={"prize-deletePrize-header"}
                    {...colProps}
                    span={1}
                  />

                  {form.values.prizes.map((_, index) => (
                    <Fragment key={`prize-${index}`}>
                      <Group align="start">
                        <Grid.Col
                          key={`prize-${index}-position`}
                          span={5.5}
                          styles={{ col: { alignSelf: "start" } }}
                          maw={207}
                        >
                          <NumberInput
                            {...form.getInputProps(`prizes.${index}.position`)}
                            disabled={disabled}
                          />
                        </Grid.Col>
                        <Grid.Col
                          key={`prize-${index}-numberMatchCount`}
                          span={5.5}
                          maw={207}
                        >
                          <NumberInput
                            {...form.getInputProps(
                              `prizes.${index}.numberMatchCount`,
                            )}
                            disabled={disabled}
                          />
                        </Grid.Col>
                        <Grid.Col key={`prize-${index}-deletePrize`} span={1}>
                          <Stack justify="center" align="center" h={"100%"}>
                            <ActionIcon
                              variant="filled"
                              color="red"
                              onClick={() =>
                                form.removeListItem("prizes", index)
                              }
                              disabled={
                                form.values.prizes.length <= 1 || disabled
                              }
                              mt={4}
                            >
                              <IconTrash />
                            </ActionIcon>
                          </Stack>
                        </Grid.Col>
                      </Group>
                    </Fragment>
                  ))}
                  <Group justify="start" w={"100%"}>
                    <Grid.Col span={1}>
                      <ActionIcon
                        disabled={
                          form.getValues().prizes.length >=
                            form.getValues().selectionsRequiredForEntry ||
                          disabled
                        }
                        onClick={() => {
                          if (disabled) return;
                          form.insertListItem("prizes", {
                            numberMatchCount: Math.max(
                              1,
                              Math.min(
                                ...form
                                  .getValues()
                                  .prizes.map((p) => p.numberMatchCount),
                              ) - 1,
                            ),
                            position:
                              Math.max(
                                ...form
                                  .getValues()
                                  .prizes.map((p) => p.position),
                              ) + 1,
                          } satisfies CreateOrEditFromValues["prizes"][number]);
                        }}
                      >
                        <IconPlus />
                      </ActionIcon>
                    </Grid.Col>
                  </Group>
                </Grid>
              </Grid.Col>
            </Grid>
          </form>
        </PageSection>
      }
      footer={
        <ActionButtons
          cancelDisabled={mutation.isPending}
          onCancel={onCancel}
          submitDisabled={disabled || submitDisabled(form)}
          formId={`${mode}-game-form`}
        />
      }
    />
  );

  return null;
}
