import {
  ActionIcon,
  Grid,
  GridColProps,
  Group,
  NumberInput,
  Select,
  Stack,
  TextInput,
  Title,
} from "@mantine/core";
import { DateTimePicker } from "@mantine/dates";
import { schemaResolver, useForm, UseFormReturnType } from "@mantine/form";
import { IconPlus, IconTrash } from "@tabler/icons-react";
import { useMutation } from "@tanstack/react-query";
import { useEffect } from "react";
import { Fragment } from "react/jsx-runtime";
import { useNavigate } from "react-router-dom";

import ActionButtons from "./ActionButtons";
import { allStatesWithLabel } from "../../../common/schemas";
import { toPascalCase } from "../../../common/utils/string.utils";
import FieldLabel from "../../../components/FieldLabel";
import FieldWrapper from "../../../components/FieldWrapper";
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
import { gameObjectSchema } from "../GameDetail/game.schema";

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
  loading?: boolean;
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
  loading,
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

  const isTouched = form.isTouched();
  const setValues = form.setValues;

  useEffect(() => {
    if (!isTouched) {
      setValues(initialValues);
    }
  }, [initialValues, isTouched, setValues]);

  const twoCols: GridColProps = {
    span: { xs: 12, sm: 6, md: 6, lg: 6 },
    style: { textAlign: "left" },
  };

  const threeCols: GridColProps = {
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
              <Grid.Col key={"name-col"} {...twoCols}>
                <FieldWrapper
                  name="Name"
                  required
                  description={gameObjectSchema.shape.name.description}
                  loading={loading}
                >
                  <TextInput
                    {...form.getInputProps("name")}
                    disabled={disabled}
                  />
                </FieldWrapper>
              </Grid.Col>
              <Grid.Col key={"state-col"} {...twoCols}>
                <FieldWrapper
                  name="State"
                  required
                  description={gameObjectSchema.shape.state.description}
                  loading={loading}
                >
                  <Select
                    {...form.getInputProps("state")}
                    data={Object.values(allStatesWithLabel)}
                    allowDeselect={false}
                    disabled={disabled}
                  />
                </FieldWrapper>
              </Grid.Col>
              <Grid.Col key={"startTime-col"} {...threeCols}>
                <FieldWrapper
                  name="Start time"
                  loading={loading}
                  required
                  description={gameObjectSchema.shape.startTime.description}
                >
                  <DateTimePicker
                    {...form.getInputProps("startTime")}
                    disabled={disabled}
                  />
                </FieldWrapper>
              </Grid.Col>
              <Grid.Col key={"closeTime-col"} {...threeCols}>
                <FieldWrapper
                  name="Close Time"
                  required
                  description={gameObjectSchema.shape.closeTime.description}
                  loading={loading}
                >
                  <DateTimePicker
                    {...form.getInputProps("closeTime")}
                    disabled={disabled}
                  />
                </FieldWrapper>
              </Grid.Col>
              <Grid.Col key={"drawTime-col"} {...threeCols}>
                <FieldWrapper
                  name="Draw Time"
                  required
                  description={gameObjectSchema.shape.drawTime.description}
                  loading={loading}
                >
                  <DateTimePicker
                    {...form.getInputProps("drawTime")}
                    disabled={disabled}
                  />
                </FieldWrapper>
              </Grid.Col>
              <Grid.Col key={"maxSelections-col"} {...threeCols}>
                <FieldWrapper
                  name="Selections in game"
                  required
                  description="The count of available numbers for players to select from"
                  loading={loading}
                >
                  <NumberInput
                    {...form.getInputProps("maxSelections")}
                    disabled={disabled}
                  />
                </FieldWrapper>
              </Grid.Col>
              <Grid.Col key={"selectionsRequiredForEntry-col"} {...threeCols}>
                <FieldWrapper
                  name="Selections per entry"
                  required
                  description={
                    gameObjectSchema.shape.selectionsRequiredForEntry
                      .description
                  }
                  loading={loading}
                >
                  <NumberInput
                    {...form.getInputProps("selectionsRequiredForEntry")}
                    disabled={disabled}
                  />
                </FieldWrapper>
              </Grid.Col>
              <Grid.Col key={"maxEntriesPerPlayer-col"} {...threeCols}>
                <FieldWrapper
                  name="Max entries per player"
                  required
                  description={
                    gameObjectSchema.shape.maxEntriesPerPlayer.description
                  }
                  loading={loading}
                >
                  <NumberInput
                    {...form.getInputProps("maxEntriesPerPlayer")}
                    disabled={disabled}
                  />
                </FieldWrapper>
              </Grid.Col>
              <Grid.Col key={"prizes-col"} span={12}>
                <Title key={"prizes-title"} size={"h2"}>
                  Prizes
                </Title>
                <Grid key={"prizes-grid"} maw={500} mx="auto" mt={20}>
                  <Grid.Col
                    key={"prize-position-header"}
                    {...twoCols}
                    span={5.5}
                  >
                    <FieldLabel
                      name="Position"
                      required
                      description={
                        gameObjectSchema.shape.prizes.element.shape.position
                          .description
                      }
                      loading={loading}
                    />
                  </Grid.Col>
                  <Grid.Col
                    key={"prize-numberMatchCount-header"}
                    {...twoCols}
                    span={5.5}
                  >
                    <FieldLabel
                      name="Matching Numbers"
                      required
                      description={
                        gameObjectSchema.shape.prizes.element.shape
                          .numberMatchCount.description
                      }
                      loading={loading}
                    />
                  </Grid.Col>
                  <Grid.Col
                    key={"prize-deletePrize-header"}
                    {...twoCols}
                    span={1}
                  />

                  {form.values.prizes.map((_, index) => (
                    <Fragment key={`prize-${index}`}>
                      <Grid.Col
                        key={`prize-${index}-position`}
                        span={5.5}
                        styles={{ col: { alignSelf: "start" } }}
                      >
                        <FieldWrapper loading={loading}>
                          <NumberInput
                            {...form.getInputProps(`prizes.${index}.position`)}
                            disabled={disabled}
                          />
                        </FieldWrapper>
                      </Grid.Col>
                      <Grid.Col
                        key={`prize-${index}-numberMatchCount`}
                        span={5.5}
                      >
                        <FieldWrapper loading={loading}>
                          <NumberInput
                            {...form.getInputProps(
                              `prizes.${index}.numberMatchCount`,
                            )}
                            disabled={disabled}
                          />
                        </FieldWrapper>
                      </Grid.Col>
                      <Grid.Col key={`prize-${index}-deletePrize`} span={1}>
                        <Stack justify="center" align="center" h={"100%"}>
                          <FieldWrapper loading={loading}>
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
                          </FieldWrapper>
                        </Stack>
                      </Grid.Col>
                    </Fragment>
                  ))}
                  <Group justify="start" w={"100%"}>
                    <Grid.Col span={1}>
                      <FieldWrapper loading={loading}>
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
                      </FieldWrapper>
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
}
