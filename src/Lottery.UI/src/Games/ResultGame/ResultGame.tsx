import { Button, Group, Modal, Paper, Stack, Tabs, Text } from "@mantine/core";
import { useState } from "react";
import SelectionPicker from "../GameDetail/SelectionPicker";
import { useMutation } from "@tanstack/react-query";
import gameService from "../gameService";

const TabTypes = {
  Manual: "manual",
  Auto: "auto",
} as const;
type TabType = (typeof TabTypes)[keyof typeof TabTypes];

interface ResultGameProps {
  numbersRequired: number;
  /**
   * The selections in the game.
   */
  selectionNumbers: Array<number>;
  gameId: string;
  onDone(): void;
  isOpen: boolean;
}

function ResultGameModal({
  numbersRequired,
  selectionNumbers,
  gameId,
  onDone,
  isOpen,
}: ResultGameProps) {
  const [activeTab, setActiveTab] = useState<TabType>("auto");
  const [selectedNumbers] = useState([]);

  const mutation = useMutation({
    mutationFn: gameService.resultGame,
    onSuccess: () => null,
  });

  function handleSubmit(selectedNumbers: Array<number> = []) {
    mutation.mutateAsync({
      body: {
        winningSelections:
          selectedNumbers?.map((selectionNumber) => ({
            selectionNumber,
          })) ?? [],
      },
      route: { gameId },
    });
  }

  return (
    <Modal opened={isOpen} onClose={onDone}>
      <Stack>
        <Group justify="center">
          <Text>Result Game</Text>
        </Group>
        <Tabs value={activeTab} onChange={(v) => setActiveTab(v as TabType)}>
          <Tabs.List grow>
            <Tabs.Tab value={TabTypes.Auto}>Auto</Tabs.Tab>
            <Tabs.Tab value={TabTypes.Manual}>Manual</Tabs.Tab>
          </Tabs.List>
        </Tabs>
        <Paper shadow="xl" p={24} radius={10}>
          {activeTab == "auto" ? (
            <AutoResultDetail onSubmit={() => handleSubmit([])} />
          ) : (
            <ManualResultDetail
              numbersRequired={numbersRequired}
              selectionNumbers={selectionNumbers}
              selectedNumbers={selectedNumbers}
              onSubmit={handleSubmit}
              onDone={onDone}
              submitStatus={
                mutation.isPending
                  ? "submitting"
                  : mutation.isSuccess
                    ? "success"
                    : "waiting"
              }
            />
          )}
        </Paper>
      </Stack>
    </Modal>
  );
}

function AutoResultDetail({ onSubmit }: { onSubmit(): void }) {
  return (
    <Stack>
      <Group justify="center">
        <Text span>Results will randomly be drawn on the server</Text>
      </Group>
      <Group justify="center">
        <Button onClick={onSubmit}>Submit</Button>
      </Group>
    </Stack>
  );
}

interface ManualResultDetailProps {
  numbersRequired: number;
  selectionNumbers: Array<number>;
  selectedNumbers: Array<number>;
  onSubmit(selectedNumbers: Array<number>): void;
  submitStatus: "waiting" | "submitting" | "success";
  onDone(): void;
}

function ManualResultDetail({
  numbersRequired,
  selectionNumbers,
  selectedNumbers,
  onSubmit,
  submitStatus,
  onDone,
}: ManualResultDetailProps) {
  return (
    <Stack>
      <Group justify="center">
        <Text span>Select the numbers to be used as the results</Text>
      </Group>
      <SelectionPicker
        requiredCount={numbersRequired}
        onDone={onDone}
        selectedNumbers={selectedNumbers}
        selectionNumbers={selectionNumbers}
        onSubmit={onSubmit}
        submitStatus={submitStatus}
      />
    </Stack>
  );
}

export default ResultGameModal;
