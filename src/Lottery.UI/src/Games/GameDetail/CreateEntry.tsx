import {
  Badge,
  BadgeProps,
  Button,
  Collapse,
  Flex,
  Group,
  Stack,
  Text,
  Title,
  useComputedColorScheme,
  useMantineTheme,
} from "@mantine/core";
import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { CreateEntryRequest, CreateEntryResponse } from "./game.schema";
import gameService from "../gameService";
import { useDisclosure } from "@mantine/hooks";
import { IconChevronDown, IconChevronUp } from "@tabler/icons-react";
import { useUserStore } from "../../stores/user.store";

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
  const [opened, { toggle }] = useDisclosure(false);
  const theme = useMantineTheme();
  const colorScheme = useComputedColorScheme();
  const user = useUserStore();
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

  function getSelectionStyle(selectionId: string): BadgeProps {
    return {
      circle: true,
      size:"xl",
      color: selectedIds.includes(selectionId) 
        ? theme.colors.green[colorScheme == "light" ? 6 : 8] 
        : selectedIds.length === selectionsRequired 
          ? theme.colors.gray[colorScheme == "light" ? 2 : 8] 
          : 'gray', 
      style: {
        cursor: selectedIds.length === selectionsRequired ?  "not-allowed" : 'default'
      }
    }
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

  const header = !user.authenticated ? (
    "Sign in to pick your numbers"
  ) : selectionsRequired - selectedIds.length ? (
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
      <Group style={{ alignSelf: "start" }} onClick={toggle}>
        <Title size={"h2"}>{`Pick your numbers`}</Title>
        {opened ? <IconChevronUp /> : <IconChevronDown />}
      </Group>
      <Collapse in={opened}>
      <Stack align="center">
        {header}
        <Flex gap={"lg"} align={"center"} justify={'center'} maw={500} wrap={'wrap'}>
        {selections
          ?.sort((a, b) => a.selectionNumber - b.selectionNumber)
          .map((selection) => (
              <Badge
                key={selection.id} 
                {...getSelectionStyle(selection.id)} 
                onClick={() => handleSelected(selection.id)}>
                {selection.selectionNumber}
              </Badge>
          ))}
        </Flex>
        </Stack>
      </Collapse>
    </Stack>
  );
}

export default CreateEntry;
