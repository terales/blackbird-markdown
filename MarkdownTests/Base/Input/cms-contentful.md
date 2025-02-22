---
title: CMS Workflows - Contentful
description: Learn how to build powerful workflows around CMS apps. In this guide we take a closer look at Contentful.
sidebar:
  label: CMS Workflows - Contentful
  order: 11
  hidden: false
---

CMSs (CMS) often serve as central hubs for managing content that may need localization or other types of processing. If you're using Blackbird, there's a good chance you'll want to integrate it with a CMS. This guide will help you understand how to build workflows that center around CMS usage.

While e-commerce platforms or product information management (PIM) systems are not officially considered CMSs, many of them offer similar features. As a result, the guidance in this document applies to these systems as well.

We'll start by exploring the common features of CMSs and the challenges they present for localization. Then, using the Contentful app as an example, we’ll walk through various strategies for CMS localization workflows. These strategies can be applied to any CMS app available on Blackbird.

Now let's begin!

The first thing you want to ask yourself when approaching a CMS workflow is the following: 

>_Does this CMS support localization?_ 

From our experience the answer can be one of three options:

1. Yes ([Contentful](/apps/contentful), [Zendesk guides](/apps/zendesk), [Sitecore](/apps/sitecore), [Hubspot blog posts & pages](/apps/hubspot-cms), etc.)
2. Yes, but only with the support of a popular plugin ([WordPress](/apps/wordpress), Drupal, etc.)
3. No ([Marketo](/apps/marketo), [Notion](/apps/notion), [Hubspot forms & emails](/apps/hubspot-cms), etc.)

When your CMS falls in the second or third category, a little more 'solution architecting' will have to take place in order to build the best worklfow possible with your CMS. You can also see that some apps only partially support localization natively (Hubspot), this gives extra challenges when localizing all possible content is desired.

This guide will from now on focus on the first (and most straighforward) of the three options. Later guides will take a closer look at the other options and solutions but will built on what is written here.

## 1. Concepts

A content management system generally holds (mainly textual) content that is grouped into an **entity**. This entity is system dependent. Examples of entities are: an *article* in Zendesk, an *entry* in Contentful, a *product* in Shopify or a *blog post* in WordPress. But WordPress also has *pages* and Shopify also has blog posts. This means that a CMS can also have different types of localizable entities. 

What groups content together into an entity is generally defined as "that what is represented on a single page". We can therefore view this entity as synonimous with a user-facing page. Pages and entities also tend to have a certain hierarchy, usually defined as groups or **categories** in a CMS. This also makes it very easy to reason about entries in different groups or categories. E.g. "I want all pages in the FAQ category translated".

An entity contains content. That content is written in a language. Therefore, the entity should have a **locale** or language attribute (Note: this is exactly what is missing in CMSs that don't natively support localization). The locale attribute is incredibly important for us since it will most likely define from which entity we pull content and to which entity we push translations.

Finally, the CMS can also have supporting features that may be crucial to your localization workflow like **tags** or **custom fields**.

With just these concept under our belt we can move on to the next part: defining the core translation workflow.

## 2. Core translation workflow

At their core, all workflows involving CMSs will contain the following structure:

1. Pull content that needs to be translated.
2. Process (translate) the content into the desired locales.
3. Push the translated content to the correct entity and locale combination.

The 3 P's of CMS workflows will always find their way into your birds.

![Schematic](../../../assets/guides/cms/1729004201270.png)

It is up to you to make the most important decisions that, together with the 3 P's, will shape your bird:

- ❓ What content should be pulled and when?
- ❓ Into what languages should be translated?
- ❓ What app or service will process the content?

When you have decided on these aspects, you will see that Blackbird will take care of the rest, namely:

- ✔️ Automatically converting the content into an HTML file that accurately represents the entity content so it can be used for TMS in-context translation, or NMT processing.
- ✔️ Mapping language codes between different systems required to process your file.
- ✔️ Waiting for long-flying processing steps or human-in-the-loop interaction (e.g. wait till the translator completes the translation).
- ✔️ Automatically pushing translated content to the right entity ID as embedded in the HTML file.

### 2.1 Machine processing

Let's take this theoretical workflow and put it into practise. In the image below you see an example of the pull, process and push steps with their respective actions in Contentful. The **Get entry as HTML file** is used to retrieve an HTML file representing the entry. In this case, DeepL is used in order to process the file (translating it into another language). Then finally, the **Update entry from HTML file** action is used to take the translated HTML file from DeepL and push it back to Contentful. Of course, DeepL can be swapped out for any other single-action processing application and this workflow would look similarly with other CMSs.

![Core with NMT](../../../assets/guides/cms/1729083328505.png)

### 2.2 Human-in-the-orchestration

It's more than likely that mere machine processing does not satisfy your localization needs. Processing your file can of course be a multi-step process. This is almost guaranteed to be the case when there will be some form of human interaction or oversight. In the below example, we are processing the file by sending it to a Phrase TMS project and waiting for the translation to be completed. We are using three steps to achieve our desired outcome. We first create a new job, we then wait for the job to be completed using a [checkpoint](../../concepts/checkpoints). We then download the translated file from Phrase TMS before we push it back to Contentful. Any human-in-the-orchestration with any TMS or other relevant system will look similar to this.

> **💡 Note**: Checkout our [checkpoints concept guide](../../concepts/checkpoints) to learn more about checkpoints!

![Core with TMS](../../../assets/guides/cms/1729083153924.png)

## 3. Continuous localization

You have learned how the core translation workflow is typically constructed in a bird. It's time to deal with the first of three big decisions that you can fill in for yourself: ❓ *What content should be pulled and when?*. A use case that Blackbird is very suited for is continuous localization. In short, a continuous localization process triggers localization workflows whenever new content is created. You can achieve this with the right [trigger](../../concepts/triggers) in Blackbird!

For our Contentful core translation workflow, all we actually need to do is create an event that is triggered when new content is created (or published in our case). Then we point the **Get entry as HTML file** to the entry ID we receive from the event.

![Continuous localization](../../../assets/guides/cms/continuous.gif)

That's it! Continuous localization checked off. ✔️

The critical reader, Contentful veteran or both, will point out a small flaw in the workflow we just created: when we publish our localized content, the workflow is triggered again potentially creating an infinite loop. - Well hats off to you. This is a problem that is dealt with in different ways in different CMSs. For example, in Zendesk you can filter the publication event to only listen to source language publications. However, Contentful does not have such a feature and all publications will trigger this event.

We recommend looking into the supporting features that CMSs have like **tags** or **custom fields** as mentioned earlier. A popular way to deal with this in Contentful is to use the tags system. You can add filters to the entry events in Blackbird so that only entries with a certain tag will trigger the bird. A good candidate could be *Ready for localization*. Be sure to delete the tag at the end of your workflow!

![Core with tags](../../../assets/guides/cms/1729086551991.png)

## 4. Scheduled and historical localization

It's possible that continuous localization doesn't quite tickle your fancy. Perhaps you are interested in a more traditional localization workflow where you take new translatable content on a recurring schedule, f.e. once a week. Or, maybe you want to use continuous localization but need to also process entities that were published in the past. In both cases you will want to have a different approach to ❓ *What content should be pulled and when?* The when will either be a scheduled trigger or a manual trigger (when you click the 'Fly' button in your bird). The what will have to be defined by a different action.

Every CMS has an action in the shape of *Search entities*, which you can use to search and select the exact content you want to process. It ususally comes with different filters including a *Updated from* and *Updated to* filter that you can use to select the time range in which the content is allowed to be updated.

![Scheduled memoQ](../../../assets/guides/cms/1729090495297.png)

## 5. Processing multiple languages

So far every bird we've seen only translated the content into one language. However, it's more than likely that you actually want to translate into multiple languages. In this section we are thus dealing with the question ❓ *Into what languages should be translated?*

In the easiest scenario the languages you want to translate to are pre-defined as per some agreement. Usually you can then "hardcode" these languages into the actions that require them. It's also likely that you want to be clever and get the languages as they are defined in the CMS. Most CMS apps havea **Get locales** or **Get languages* action that will return the default language and the other languages that are configured. This is perfect! Because now you can send those languages directly into your processing application.

![TMS languages](../../../assets/guides/cms/1729176014667.png)

There is a very important thing to point out when sending languages from one system to the other: they may not be using the same languages codes. That's why in the bird section above we are using the **Convert operator** to convert from Contentful language codes into memoQ language codes. You can read more about conversion and libraries in [this guide](../../concepts/libraries).

A TMS can usually take all the languages you want to convert to in a single input field, since it would create a project for your content. However, NMT and other single-step processing apps tend to only take one language at a time. In this case you will have to loop over all the languages and process them for each file (see gif below). You can find more information about loops [here](../loops).

![Continuous localization](../../../assets/guides/cms/multilocales.gif)

## 6. Contentful gotchas

Every CMS has its quircks. While we have been featuring Contentful it may be a good moment to dive into Contentful's quircks a little deeper. We recommend always consulting the [Blackbird Contentful documentation](../../apps/contentful) for the most up-to-date version.

### 6.1 Content selection

As a headless CMS, there is an inherent disconnect between how Contentful displays content and how this content is ultimatly rendered to a website and visible to the end-user. It is up to a developer to create this link but **they can choose to ignore some of Contentful's features**. This is particularly annoying when these features revolve around content and localization. Luckily, pulling and pushing content between Blackbird and Contentful can be very surgical, perhaps even more surgical than a typical connector you can find provided by a TMS.

In Contentful one can embed content from one entry into another entry. This is often used by teams using Contentful to reduce the redundancy of certain content. It is still important that when one entry is translated, **all the linked entries are translated as well**.

In Contentful you can mark an entry field as localizable. You have to explicitly set this property on each field (see images below).

![1707747998688](https://raw.githubusercontent.com/bb-io/Contentful/main/image/README/1707747998688.png)
![1707748006274](https://raw.githubusercontent.com/bb-io/Contentful/main/image/README/1707748006274.png)

The **Get entry as HTML file** action also lets you define if you want to recursively embed content (for translation) from linked entries.

There are 4 types of linked entries:
- Reference field types from the content model
- Hyperlinks that link to an entry in 'Rich text' fields
- Inline embedded entries in 'Rich text' fields
- Block embedded entries in 'Rich text' fields

In the action you are able to select exactly which type of linked entry you want to include in the exported HTML file. If you f.e. select 'Hyperlinks' and 'Inline embedded entries', we will recursively search through all 'Rich text' fields and fetch all the content of these embedded entries. For these embedded entries, we do the same thing and also get all hyperlinks and embedded inline entries, and so on.

> Note: you can also specify if you want to ignore the localization aspect of reference fields. If this optional input is true, and the 'Include referenced entries' is true, then all referenced entries will be included regardless of localization setting. This is particularly important if the developer decided to ignore the localization field.

Finally, finding embeded entries recursively but indefinitely can be quite dangerous. You may want to explain to Blackbird where and with which field types to stop. You can specify a list of Field IDs which will always be ignored and not added to the produced HTML file.

![Contentful surgical](../../../assets/guides/cms/1729093156381.png)

### 6.2 Contentful workflows

Previously we have talked about continuous localization and about batches of scheduled or historical entities. If the CMS actually natively implements features that help with the content transformation pipeline then that would be ideal of course! Luckily Contentful does have a feature that is designed for this purpose. Contentful calls it 'workflows' and we invite you to read [its documentation](https://www.contentful.com/marketplace/workflows-app/).

Workflows is designed to track content through its creation lifecycle as it changes hands between different people. Blackbird is also able to inject itself in Contentful workflows! An example of that can be seen here.

![Contentful workflows](https://raw.githubusercontent.com/bb-io/Contentful/main/image/README/1727786944492.png)

When a new task is created (we can addtionally filter on task body and assigned user) we will pull the entry related to this task as an HTML file, translate it with DeepL and update the translation in Contentful. Additionally, we update the status of the task to mark it as resolved.

Using Contentful workflows in combination with Blackbird is ideal for teams that want to exert even more control over ❓ *what content should be pulled and when?* You can use it to send entries directly from Contentful to a bird that will then orchestrate the content further and ultimately deliver it back.
