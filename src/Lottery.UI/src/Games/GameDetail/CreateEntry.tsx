import { Collapse, Group, Stack, Text, Title } from "@mantine/core";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import gameService from "../gameService";
import { useDisclosure } from "@mantine/hooks";
import { IconChevronDown, IconChevronUp } from "@tabler/icons-react";
import SelectionPicker from "./SelectionPicker";

interface CreateEntryProps {
  gameId: string;
  selectionNumbers: Array<number>;
  selectionsRequired: number;
}

function CreateEntry({
  selectionNumbers: selections,
  gameId,
  selectionsRequired,
}: CreateEntryProps) {
  const [success, setSuccess] = useState(false);
  const [selectedNumbers, setSelectedNumbers] = useState<ReadonlyArray<number>>(
    [],
  );

  const [opened, { toggle }] = useDisclosure(false);
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: gameService.createEntry,
    onSuccess: handleMutationSuccess,
  });

  function handleMutationSuccess() {
    setSuccess(true);
    queryClient.refetchQueries({ exact: false, queryKey: ["entries", gameId] });
  }

  async function handleSubmitEntry(selectionNumbers: Array<number>) {
    await mutation.mutateAsync({
      body: {
        gameId,
        selections: selectionNumbers.map((selectionNumber) => ({
          selectionNumber,
        })),
      },
    });
  }

  function handleSelectionPickerDone() {
    setSelectedNumbers([]);
    setSuccess(false);
  }

  // TODO: Improve this, remove the annoying overlay on success
  return (
    <Stack justify="center" align="center">
      <Group style={{ alignSelf: "start" }} onClick={toggle}>
        <Title size={"h2"}>{`Pick your numbers`}</Title>
        {opened ? <IconChevronUp /> : <IconChevronDown />}
      </Group>
      <Collapse expanded={opened}>
        <Stack align="center">
          <Text>{`Pick ${selectionsRequired} numbers to submit your entry`}</Text>
          <SelectionPicker
            requiredCount={selectionsRequired}
            selectedNumbers={selectedNumbers}
            selectionNumbers={selections}
            onSubmit={handleSubmitEntry}
            onDone={handleSelectionPickerDone}
            submitStatus={
              mutation.isPending
                ? "submitting"
                : success
                  ? "success"
                  : "waiting"
            }
          />
        </Stack>
      </Collapse>
    </Stack>
  );
}

export default CreateEntry;
