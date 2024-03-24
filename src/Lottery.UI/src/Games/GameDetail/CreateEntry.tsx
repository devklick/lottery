import {
  Button,
  Grid,
  MantineStyleProp,
  Stack,
  Text,
  Title,
  useComputedColorScheme,
  useMantineColorScheme,
  useMantineTheme,
} from "@mantine/core";
import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { CreateEntryRequest, CreateEntryResponse } from "./game.schema";
import gameService from "../gameService";

interface CreateEntryProps {
  gameId: string;
  selections: Array<{ id: string; selectionNumber: number }>;
  selectionsRequired: number;
}

function CreateEntry({
  selections,
  gameId,
  selectionsRequired,
}: CreateEntryProps) {
  const [selectedIds, setSelectedIds] = useState<Array<string>>([]);
  const theme = useMantineTheme();
  const colorScheme = useComputedColorScheme();
  const mutation = useMutation<
    CreateEntryResponse,
    unknown,
    CreateEntryRequest
  >({
    mutationFn: gameService.createEntry,
  });

  function handleSelected(selectionId: string) {
    const newSelectedIds = [...selectedIds];
    const i = newSelectedIds.indexOf(selectionId);
    // if already exists, remove
    if (i >= 0) {
      newSelectedIds.splice(i, 1);
    }
    // if not exists, add
    else if (newSelectedIds.length < selectionsRequired) {
      newSelectedIds.push(selectionId);
    }
    setSelectedIds(newSelectedIds);
  }

  function getSelectionStyle(selectionId: string): MantineStyleProp {
    const style: MantineStyleProp = {
      border: 1,
      borderStyle: "solid",
      borderRadius: 10,
      borderColor: theme.colors.gray[5],
      backgroundColor: "transparent",
    };

    if (selectedIds.length === selectionsRequired) {
      style.backgroundColor = theme.colors.gray[colorScheme == "light" ? 2 : 8];
      style.borderColor = theme.colors.gray[colorScheme == "light" ? 2 : 8];
      style.color = theme.colors.gray[colorScheme == "light" ? 5 : 6];
      style.cursor = "not-allowed";
    }

    if (selectedIds.includes(selectionId)) {
      style.backgroundColor =
        theme.colors.green[colorScheme == "light" ? 6 : 8];
      style.borderColor = theme.colors.green[5];
      style.color = theme.white;
      style.cursor = "default";
    }

    return style;
  }

  async function handleSubmitEntry() {
    await mutation.mutateAsync({
      body: {
        gameId,
        selections: selections
          .filter((s) => selectedIds.includes(s.id))
          .map((s) => ({ selectionNumber: s.selectionNumber })),
      },
    });
  }

  const header =
    selectionsRequired - selectedIds.length ? (
      <Text span>{`${
        selectionsRequired - selectedIds.length
      } out of ${selectionsRequired} remaining`}</Text>
    ) : (
      <Button h={24.8} onClick={handleSubmitEntry}>
        {mutation.isPending ? "Submitting" : "Submit"}
      </Button>
    );

  return (
    <Stack justify="center" align="center" mt={50}>
      <Title size={"h2"}>{`Pick your numbers`}</Title>
      {header}
      <Grid maw={500} justify="center">
        {selections
          ?.sort((a, b) => a.selectionNumber - b.selectionNumber)
          .map((selection) => (
            <Grid.Col
              key={`grid-selection-${selection.selectionNumber}`}
              span={2}
              onClick={() => handleSelected(selection.id)}
            >
              <Stack style={{ ...getSelectionStyle(selection.id) }}>
                <Text span style={{ lineHeight: 1, paddingTop: 5 }}>
                  {selection.selectionNumber}
                </Text>
              </Stack>
            </Grid.Col>
          ))}
      </Grid>
    </Stack>
  );
}

export default CreateEntry;
